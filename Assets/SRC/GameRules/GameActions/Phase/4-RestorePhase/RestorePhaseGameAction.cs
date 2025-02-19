using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class RestorePhaseGameAction : PhaseGameAction, IPhase
    {
        public override Phase MainPhase => Phase.Restore;
        public override Localization PhaseNameLocalization => new("PhaseName_RestorePhase");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_RestorePhase");

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<ResetAllInvestigatorsTurnsGameAction>().Execute();
            await _gameActionsProvider.Create<ReadyAllCardsGameAction>().Execute();
            await _gameActionsProvider.Create<AllInvestigatorsDrawCardAndResourceGameAction>().Execute();
            await _gameActionsProvider.Create<AllInvestigatorsCheckHandSizeGameAction>().Execute();
        }
    }

}
