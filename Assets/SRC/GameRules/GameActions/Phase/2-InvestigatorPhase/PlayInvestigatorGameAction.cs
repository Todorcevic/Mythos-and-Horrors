using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PlayInvestigatorGameAction : PhaseGameAction
    {
        public override Phase MainPhase => Phase.Investigator;
        public override bool CanBeExecuted => ActiveInvestigator.HasTurnsAvailable.IsTrue;
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
            await _gameActionsProvider.Create<InvestigatorTurnGameAction>().SetWith(ActiveInvestigator).Execute();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ActiveInvestigator.IsPlayingHisTurn, false).Execute();

            //await _gameActionsProvider.Create<SafeWhile>().SetWith(() => ActiveInvestigator.HasTurnsAvailable.IsTrue, PlayTurn).Execute();

            /*******************************************************************/
            //async Task PlayTurn() => await _gameActionsProvider.Create<InvestigatorTurnGameAction>().SetWith(ActiveInvestigator).Execute();
        }
    }
}
