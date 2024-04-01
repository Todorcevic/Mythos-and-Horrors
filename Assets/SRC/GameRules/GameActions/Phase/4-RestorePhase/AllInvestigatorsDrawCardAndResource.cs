using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class AllInvestigatorsDrawCardAndResource : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(AllInvestigatorsDrawCardAndResource);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(AllInvestigatorsDrawCardAndResource);
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        public override bool CanBeExecuted => ActiveInvestigator != null;

        /*******************************************************************/
        public AllInvestigatorsDrawCardAndResource(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new DrawAidGameAction(ActiveInvestigator));
            await _gameActionsProvider.Create(new GainResourceGameAction(ActiveInvestigator, 1));
            await _gameActionsProvider.Create(new AllInvestigatorsDrawCardAndResource(ActiveInvestigator.NextInvestigatorInPlay));
        }
    }
}
