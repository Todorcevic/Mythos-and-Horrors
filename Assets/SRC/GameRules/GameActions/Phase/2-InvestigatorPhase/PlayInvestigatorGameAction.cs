using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PlayInvestigatorGameAction : PhaseGameAction
    {
        private Investigator lastInvestigator;

        public override Phase MainPhase => Phase.Investigator;
        public override bool CanBeExecuted => ActiveInvestigator.HasTurnsAvailable.IsTrue;
        public static Investigator PlayActiveInvestigator { get; private set; }
        public override Localization PhaseNameLocalization => new("PhaseName_PlayInvestigator");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_PlayInvestigator");

        /*******************************************************************/
        public PlayInvestigatorGameAction SetWith(Investigator investigator)
        {
            lastInvestigator = PlayActiveInvestigator;
            PlayActiveInvestigator = ActiveInvestigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ActiveInvestigator.IsPlayingTurns, true).Execute();
            await _gameActionsProvider.Create<InvestigatorTurnGameAction>().SetWith().Execute();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ActiveInvestigator.IsPlayingTurns, false).Execute();
        }

        public override async Task Undo()
        {
            PlayActiveInvestigator = lastInvestigator;
            await base.Undo();
        }
    }
}
