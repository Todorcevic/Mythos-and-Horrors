using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorScenePhaseDrawDangerGameAction : PhaseGameAction
    {
        public override Localization PhaseNameLocalization => new("PhaseName_InvestigatorsDrawDangerCard");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_InvestigatorsDrawDangerCard");
        public override Phase MainPhase => Phase.Scene;

        /*******************************************************************/
        public InvestigatorScenePhaseDrawDangerGameAction SetWith(Investigator investigator)
        {
            ActiveInvestigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<DrawDangerGameAction>().SetWith(ActiveInvestigator).Execute();
        }
    }
}
