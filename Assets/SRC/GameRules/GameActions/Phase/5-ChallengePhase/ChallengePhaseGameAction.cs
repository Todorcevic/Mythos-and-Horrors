using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChallengePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly IPresenter<ChallengePhaseGameAction> _challengerPresenter;
        [Inject] private readonly IPresenter<PhaseGameAction> _changePhasePresenter;

        public Stat Stat { get; init; }
        public int DifficultValue { get; init; }
        public string ChallengeName { get; init; }
        public Func<Task> SuccesEffect { get; init; }
        public Func<Task> FailEffect { get; init; }
        public Card CardToChallenge { get; init; }
        public List<ChallengeToken> TokensRevealed { get; private set; } = new();
        public bool IsSuccessful { get; private set; }

        public override Investigator ActiveInvestigator => _investigatorsProvider.GetInvestigatorWithThisStat(Stat);
        public ChallengeType ChallengeType => ActiveInvestigator.GetChallengeType(Stat);

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ChallengePhaseGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ChallengePhaseGameAction);
        public override Phase MainPhase => Phase.Challenge;

        /*******************************************************************/
        public ChallengePhaseGameAction(Stat stat, int difficultValue, string name, Func<Task> succesEffect = null, Func<Task> failEffect = null, Card cardToChallenge = null)
        {
            Stat = stat;
            DifficultValue = difficultValue;
            ChallengeName = name;
            SuccesEffect = succesEffect;
            FailEffect = failEffect;
            CardToChallenge = cardToChallenge;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _challengerPresenter.PlayAnimationWith(this);
            await _gameActionsProvider.Create(new CommitCardsChallengeGameAction(Stat, DifficultValue));
            ChallengeToken revealToken = (await _gameActionsProvider.Create(new RevealChallengeTokenGameAction())).ChallengeTokenRevealed;
            TokensRevealed.Add(revealToken);
            await _gameActionsProvider.Create(new ResolveMultiChallengeTokensGamaAction(TokensRevealed));
            IsSuccessful = (await _gameActionsProvider.Create(new ResultChallengeGameAction(Stat, DifficultValue, revealToken, ChallengeType))).IsSuccessful;
            await _gameActionsProvider.Create(new ResolveChallengeGameAction(IsSuccessful, SuccesEffect, FailEffect));
            await _gameActionsProvider.Create(new FinishChallengeGameAction());
            await _changePhasePresenter.PlayAnimationWith(_gameActionsProvider.GetRealCurrentPhase());
        }
    }
}
