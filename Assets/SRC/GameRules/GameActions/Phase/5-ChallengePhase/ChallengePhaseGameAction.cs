using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChallengePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly IPresenter<ChallengePhaseGameAction> _challengerPresenter;
        [Inject] private readonly IPresenter<PhaseGameAction> _changePhasePresenter;

        public Stat Stat { get; private set; }
        public int InitialDifficultValue { get; private set; }
        public Stat StatModifier { get; private set; }
        public string ChallengeName { get; private set; }
        public List<Func<Task>> SuccesEffects { get; init; } = new();
        public List<Func<Task>> FailEffects { get; init; } = new();
        public Card CardToChallenge { get; private set; }
        public bool IsAutoSucceed { get; set; }
        public bool IsAutoFail { get; set; }
        public ResultChallengeGameAction ResultChallenge { get; private set; }

        public override Investigator ActiveInvestigator => _investigatorsProvider?.GetInvestigatorWithThisStat(Stat) ?? throw new NullReferenceException("_investigatorsProvider must be Inject");
        public ChallengeType ChallengeType => ActiveInvestigator.GetChallengeType(Stat);

        private IEnumerable<ChallengeToken> CurrentTokensRevealed => _challengeTokensProvider.ChallengeTokensRevealed;
        public int CurrentTotalTokenValue => CurrentTokensRevealed.Sum(token => token.Value(ActiveInvestigator));
        public int CurrentTotalChallengeValue => IsAutoFail ? 0 : Stat.Value + StatModifier.Value + CurrentTotalTokenValue + CurrentCommitsCards.Sum(commitableCard => commitableCard.GetChallengeValue(ChallengeType));
        public IEnumerable<CommitableCard> CurrentCommitsCards => _chaptersProvider.CurrentScene.LimboZone.Cards.OfType<CommitableCard>()
            .Where(comitableCard => comitableCard.Commited.IsActive);
        public int DifficultValue => IsAutoSucceed ? 0 : InitialDifficultValue;
        public bool IsSucceed => (bool)ResultChallenge?.IsSuccessful;

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ChallengePhaseGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ChallengePhaseGameAction);
        public override Phase MainPhase => Phase.Challenge;

        /*******************************************************************/
        public ChallengePhaseGameAction SetWith(Stat stat, int difficultValue, string name, Card cardToChallenge, Func<Task> succesEffect = null, Func<Task> failEffect = null)
        {
            Stat = stat;
            StatModifier = new Stat(0, true);
            InitialDifficultValue = difficultValue;
            CardToChallenge = cardToChallenge;
            ChallengeName = name;
            if (succesEffect != null) SuccesEffects.Add(succesEffect);
            if (failEffect != null) FailEffects.Add(failEffect);
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _challengerPresenter.PlayAnimationWith(this);
            await _gameActionsProvider.Create<CommitCardsChallengeGameAction>().SetWith(this).Start();
            await _changePhasePresenter.PlayAnimationWith(_gameActionsProvider.GetRealCurrentPhase() ?? this);
        }

        public async Task ContinueChallenge()
        {
            await _gameActionsProvider.Create<RevealRandomChallengeTokenGameAction>().SetWith(ActiveInvestigator).Start();
            await _gameActionsProvider.Create<ResolveAllTokensGameAction>().SetWith(ActiveInvestigator).Start();
            ResultChallenge = _gameActionsProvider.Create<ResultChallengeGameAction>().SetWith(this);
            await ResultChallenge.Start();
            await _challengerPresenter.PlayAnimationWith(this);
            await _gameActionsProvider.Create(new RestoreAllChallengeTokens());
            await _gameActionsProvider.Create<ResolveChallengeGameAction>().SetWith(this).Start();
            await _gameActionsProvider.Create(new DiscardCommitsCards());
        }

        public void ChangeStat(Stat stat) => Stat = stat;

        /*******************************************************************/
        public bool IsUndo { get; private set; }
        public override async Task Undo()
        {
            IsUndo = true;
            await base.Undo();
            await _challengerPresenter.PlayAnimationWith(this);
        }
    }
}
