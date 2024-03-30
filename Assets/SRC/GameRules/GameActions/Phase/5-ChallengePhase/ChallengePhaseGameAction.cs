using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChallengePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly IPresenter<ChallengePhaseGameAction> _challengerPresenter;

        public Stat Stat { get; init; }
        public int DifficultValue { get; init; }
        public ChallengeType ChallengeType { get; init; }
        public Func<Task> SuccesEffect { get; init; }
        public Func<Task> FailEffect { get; init; }
        public Card CardToChallenge { get; init; }

        public List<ChallengeToken> TokensRevealed { get; private set; } = new();
        public bool IsSuccessful { get; private set; }

        /*******************************************************************/
        public override Phase MainPhase => Phase.Challenge;
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ChallengePhaseGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ChallengePhaseGameAction);

        /*******************************************************************/
        public ChallengePhaseGameAction(Stat stat, int difficultValue, Func<Task> succesEffect = null, Func<Task> failEffect = null, Card cardToChallenge = null)
        {
            Stat = stat;
            DifficultValue = difficultValue;
            SuccesEffect = succesEffect;
            FailEffect = failEffect;
            CardToChallenge = cardToChallenge;
            ActiveInvestigator = _investigatorsProvider.GetInvestigatorWithThisStat(Stat);
            ChallengeType = ActiveInvestigator.GetChallengeType(Stat);
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
        }
    }
}
