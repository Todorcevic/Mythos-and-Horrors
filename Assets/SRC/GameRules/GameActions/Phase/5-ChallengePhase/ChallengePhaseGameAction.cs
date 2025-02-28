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
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        public Stat Stat { get; private set; }
        public int InitialDifficultValue { get; private set; }
        public Stat StatModifier { get; private set; }
        public Localization ChallengeName { get; private set; }
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
        public int CurrentTotalChallengeValue => IsAutoFail ? 0 : Stat.Value + StatModifier.Value + CurrentTotalTokenValue + CurrentCommitsCards.Sum(commitableCard => commitableCard.GetChallengeFullValueWithWild(ChallengeType));
        public IEnumerable<CommitableCard> CurrentCommitsCards => _chaptersProvider.CurrentScene.LimboZone.Cards.OfType<CommitableCard>()
            .Where(comitableCard => comitableCard.Commited.IsActive);
        public int DifficultValue => IsAutoSucceed ? 0 : InitialDifficultValue;
        public bool IsSucceed => (bool)ResultChallenge?.IsSuccessful;

        /*******************************************************************/
        public override Localization PhaseNameLocalization => new("PhaseName_Challenge");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_Challenge");
        public override Phase MainPhase => Phase.Challenge;

        /*******************************************************************/
        public ChallengePhaseGameAction SetWith(Stat stat, int difficultValue, Localization localization, Card cardToChallenge, Func<Task> succesEffect = null, Func<Task> failEffect = null)
        {
            Stat = stat;
            StatModifier = new Stat(0, true);
            InitialDifficultValue = difficultValue;
            CardToChallenge = cardToChallenge;
            ChallengeName = localization;
            if (succesEffect != null) SuccesEffects.Add(succesEffect);
            if (failEffect != null) FailEffects.Add(failEffect);
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<CommitCardsChallengeGameAction>().SetWith(this).Execute();
            await _gameActionsProvider.Create<RevealRandomChallengeTokenGameAction>().SetWith(ActiveInvestigator).Execute();
            await _gameActionsProvider.Create<ResolveAllTokensGameAction>().SetWith(ActiveInvestigator).Execute();
            ResultChallenge = _gameActionsProvider.Create<ResultChallengeGameAction>().SetWith(this);
            await ResultChallenge.Execute();
            await _gameActionsProvider.Create<FinishChallengeControlGameAction>().SetWith(this).Execute();
            await _gameActionsProvider.Create<RestoreAllChallengeTokensGameAction>().Execute();
            await _gameActionsProvider.Create<ResolveChallengeGameAction>().SetWith(this).Execute();
            await _gameActionsProvider.Create<DiscardCommitsCardsGameAction>().Execute();
        }

        public void ChangeStat(Stat stat) => Stat = stat;

        /*******************************************************************/
        public bool IsUndo { get; private set; }
        public override async Task Undo()
        {
            IsUndo = true;
            await base.Undo();
        }
    }
}
