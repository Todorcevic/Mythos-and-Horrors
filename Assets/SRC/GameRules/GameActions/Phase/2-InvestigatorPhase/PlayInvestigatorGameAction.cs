using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PlayInvestigatorGameAction : PhaseGameAction
    {
        public override Phase MainPhase => Phase.Investigator;
        public override bool CanBeExecuted => ActiveInvestigator.IsInPlay.IsTrue;
        public override Localization PhaseNameLocalization => new("PhaseName_PlayInvestigator");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_PlayInvestigator");

        /*******************************************************************/
        public PlayInvestigatorGameAction SetWith(Investigator investigator)
        {
            ActiveInvestigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ActiveInvestigator.IsPlayingHisTurn, true).Execute();
            await _gameActionsProvider.Create<SafeWhile>().SetWith(MainOrUndoButtonIsNotPressed, ExecuteInvestigatorTurn).Execute();

            /*******************************************************************/
            bool MainOrUndoButtonIsNotPressed() => ActiveInvestigator.IsInPlay.IsTrue && ActiveInvestigator.IsPlayingHisTurn.IsActive;
            async Task ExecuteInvestigatorTurn() => await _gameActionsProvider.Create<InvestigatorTurnGameAction>().SetWith(ActiveInvestigator).Execute();
        }
    }
}
