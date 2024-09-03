using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class RestorePhaseGameAction : PhaseGameAction, IPhase
    {
        public override Phase MainPhase => Phase.Restore;
        public override Localization PhaseNameLocalization => new("PhaseName_RestorePhase");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_RestorePhase");

        /*******************************************************************/
        //4.1	Upkeep phase begins.
        protected override async Task ExecuteThisPhaseLogic()
        {
            //4.2	Reset actions.
            await _gameActionsProvider.Create<ResetAllInvestigatorsTurnsGameAction>().Execute();
            //4.3	Ready all exhausted cards.
            await _gameActionsProvider.Create<ReadyAllCardsGameAction>().Execute();
            //4.4	Each investigator draws 1 card and gains 1 resource.
            await _gameActionsProvider.Create<AllInvestigatorsDrawCardAndResourceGameAction>().Execute();
            //4.5	Each investigator checks hand size.
            await _gameActionsProvider.Create<AllInvestigatorsCheckHandSizeGameAction>().Execute();
        }
        //4.6	Upkeep phase ends.
    }
}
