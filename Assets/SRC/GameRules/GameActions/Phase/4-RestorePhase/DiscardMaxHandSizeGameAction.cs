using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DiscardMaxHandSizeGameAction : PhaseGameAction
    {
        public override Localization PhaseNameLocalization => new("PhaseName_RestorePhase");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_RestorePhase");
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        public DiscardMaxHandSizeGameAction SetWith(Investigator investigator)
        {
            ActiveInvestigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<CheckMaxHandSizeGameAction>().SetWith(ActiveInvestigator, ActiveInvestigator.DiscardableCardsInHand).Execute();
        }
    }

}
