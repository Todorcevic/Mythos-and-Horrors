using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class AllInvestigatorsCheckHandSize : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(AllInvestigatorsCheckHandSize);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(AllInvestigatorsCheckHandSize);
        public override Phase MainPhase => Phase.Restore;
        public override bool CanBeExecuted => ActiveInvestigator != null;

        /*******************************************************************/
        public AllInvestigatorsCheckHandSize(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new CheckMaxHandSizeGameAction(ActiveInvestigator));
            await _gameActionsProvider.Create(new AllInvestigatorsCheckHandSize(ActiveInvestigator.NextInvestigatorInPlay));
        }
    }
}
