using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChallengePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly IPresenter<ChallengePhaseGameAction> _challengerPresenter;
        [Inject] private readonly IPresenter<PhaseGameAction> _changePhasePresenter;

        public Stat Stat { get; init; }
        public int InitialDifficultValue { get; init; }
        public string ChallengeName { get; init; }
        public List<Func<Task>> SuccesEffects { get; init; } = new();
        public List<Func<Task>> FailEffects { get; init; } = new();
        public Card CardToChallenge { get; init; }

        public bool? IsSuccessful { get; set; }
        public bool IsAutoSucceed { get; set; }
        public bool IsAutoFail { get; set; }

        public IEnumerable<ChallengeToken> TokensRevealed => _challengeTokensProvider.ChallengeTokensRevealed;
        public override Investigator ActiveInvestigator => _investigatorsProvider.GetInvestigatorWithThisStat(Stat);
        public ChallengeType ChallengeType => ActiveInvestigator.GetChallengeType(Stat);
        public IEnumerable<ICommitable> CommitsCards => _chaptersProvider.CurrentScene.LimboZone.Cards.OfType<ICommitable>();
        public int TotalTokenValue => TokensRevealed.Sum(token => token.Value(ActiveInvestigator));
        public int TotalChallengeValue => IsAutoFail ? 0 : Stat.Value + TotalTokenValue + CommitsCards.Sum(commitableCard => commitableCard.GetChallengeValue(ChallengeType));
        public int DifficultValue => IsAutoSucceed ? 0 : InitialDifficultValue;

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ChallengePhaseGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ChallengePhaseGameAction);
        public override Phase MainPhase => Phase.Challenge;

        /*******************************************************************/
        public ChallengePhaseGameAction(Stat stat, int difficultValue, string name, Func<Task> succesEffect = null, Func<Task> failEffect = null, Card cardToChallenge = null)
        {
            Stat = stat;
            InitialDifficultValue = difficultValue;
            ChallengeName = name;
            if (succesEffect != null) SuccesEffects.Add(succesEffect);
            if (failEffect != null) FailEffects.Add(failEffect);
            CardToChallenge = cardToChallenge;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _challengerPresenter.PlayAnimationWith(this);
            await _gameActionsProvider.Create(new CommitCardsChallengeGameAction(ActiveInvestigator, this));
            await _changePhasePresenter.PlayAnimationWith(_gameActionsProvider.GetRealCurrentPhase() ?? this);
        }

        public async Task ContinueChallenge()
        {
            await _gameActionsProvider.Create(new RevealRandomChallengeTokenGameAction(ActiveInvestigator));
            await _gameActionsProvider.Create(new ResolveAllTokensGameAction(ActiveInvestigator));
            await _gameActionsProvider.Create(new ResultChallengeGameAction(this));
            await _gameActionsProvider.Create(new RestoreAllChallengeTokens());
            await _gameActionsProvider.Create(new ResolveChallengeGameAction(this));
            await _gameActionsProvider.Create(new DiscardCommitsCards());
        }

        public bool IsUndo { get; private set; }
        public override async Task Undo()
        {
            IsUndo = true;
            await _challengerPresenter.PlayAnimationWith(this);
        }
    }
}
