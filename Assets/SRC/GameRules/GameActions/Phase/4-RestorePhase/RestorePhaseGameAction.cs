using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RestorePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override Phase MainPhase => Phase.Restore;
        public override string Name => _textsProvider.GameText.RESTORE_PHASE_NAME;
        public override string Description => _textsProvider.GameText.RESTORE_PHASE_DESCRIPTION;

        /*******************************************************************/
        //4.1	Upkeep phase begins.
        protected override async Task ExecuteThisPhaseLogic()
        {
            //4.2	Reset actions.
            await _gameActionsProvider.Create(new ResetAllInvestigatorsTurnsGameAction());
            //4.3	Ready all exhausted cards.
            await _gameActionsProvider.Create(new ReadyAllCardsGameAction());
            //4.4	Each investigator draws 1 card and gains 1 resource.
            await _gameActionsProvider.Create(new AllInvestigatorsDrawCardAndResource());
            //4.5	Each investigator checks hand size.
            await _gameActionsProvider.Create(new AllInvestigatorsCheckHandSize());
        }
        //4.6	Upkeep phase ends.
    }
}
