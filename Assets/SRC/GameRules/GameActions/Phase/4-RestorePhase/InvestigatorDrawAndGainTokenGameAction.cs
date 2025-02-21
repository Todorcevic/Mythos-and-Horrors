using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorDrawAndGainTokenGameAction : PhaseGameAction
    {
        public override Localization PhaseNameLocalization => new("PhaseName_AllInvestigatorsDrawCardAndResource");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_AllInvestigatorsDrawCardAndResource");
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        public InvestigatorDrawAndGainTokenGameAction SetWith(Investigator investigator)
        {
            ActiveInvestigator = investigator;
            return this;
        }

        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(ActiveInvestigator).Execute();
            await _gameActionsProvider.Create<GainResourceGameAction>().SetWith(ActiveInvestigator, 1).Execute();
        }
    }
}
