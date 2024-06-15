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

        public Stat Stat { get; private set; }
        public int InitialDifficultValue { get; init; }
        public Stat StatModifier { get; private set; }
        public string ChallengeName { get; init; }
        public List<Func<Task>> SuccesEffects { get; init; } = new();
        public List<Func<Task>> FailEffects { get; init; } = new();
        public Card CardToChallenge { get; init; }
        public bool IsAutoSucceed { get; set; }
        public bool IsAutoFail { get; set; }
        public ResultChallengeGameAction ResultChallenge { get; private set; }

        public override Investigator ActiveInvestigator => _investigatorsProvider.GetInvestigatorWithThisStat(Stat);
        public ChallengeType ChallengeType => ActiveInvestigator.GetChallengeType(Stat);

        private IEnumerable<ChallengeToken> CurrentTokensRevealed => _challengeTokensProvider.ChallengeTokensRevealed;
        public int CurrentTotalTokenValue => CurrentTokensRevealed.Sum(token => token.Value(ActiveInvestigator));
        public int CurrentTotalChallengeValue => IsAutoFail ? 0 : Stat.Value + CurrentTotalTokenValue + CurrentCommitsCards.Sum(commitableCard => commitableCard.GetChallengeValue(ChallengeType));
        public IEnumerable<ICommitable> CurrentCommitsCards => _chaptersProvider.CurrentScene.LimboZone.Cards.OfType<ICommitable>()
            .Where(comiitable => comiitable.Commited.IsActive);
        public int DifficultValue => IsAutoSucceed ? 0 : InitialDifficultValue;
        public bool? IsSuccessful => ResultChallenge?.IsSuccessful;

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ChallengePhaseGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ChallengePhaseGameAction);
        public override Phase MainPhase => Phase.Challenge;

        /*******************************************************************/
        public ChallengePhaseGameAction(Stat stat, int difficultValue, string name, Card cardToChallenge, Func<Task> succesEffect = null, Func<Task> failEffect = null, int statModifier = 0)
        {
            Stat = stat;
            InitialDifficultValue = difficultValue;
            ChallengeName = name;
            if (succesEffect != null) SuccesEffects.Add(succesEffect);
            if (failEffect != null) FailEffects.Add(failEffect);
            CardToChallenge = cardToChallenge;
            StatModifier = new Stat(statModifier, true);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new IncrementStatGameAction(Stat, StatModifier.Value));
            await _challengerPresenter.PlayAnimationWith(this);
            await _gameActionsProvider.Create(new CommitCardsChallengeGameAction(ActiveInvestigator, this));
            await _changePhasePresenter.PlayAnimationWith(_gameActionsProvider.GetRealCurrentPhase() ?? this);
            await _gameActionsProvider.Create(new DecrementStatGameAction(Stat, StatModifier.Value));
        }

        public async Task ContinueChallenge()
        {
            await _gameActionsProvider.Create(new RevealRandomChallengeTokenGameAction(ActiveInvestigator));
            await _gameActionsProvider.Create(new ResolveAllTokensGameAction(ActiveInvestigator));
            ResultChallenge = await _gameActionsProvider.Create(new ResultChallengeGameAction(this));
            await _challengerPresenter.PlayAnimationWith(this);
            await _gameActionsProvider.Create(new RestoreAllChallengeTokens());
            await _gameActionsProvider.Create(new ResolveChallengeGameAction(this));
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
