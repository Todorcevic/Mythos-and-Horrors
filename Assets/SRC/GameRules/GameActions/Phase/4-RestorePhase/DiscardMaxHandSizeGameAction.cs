using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DiscardMaxHandSizeGameAction : PhaseGameAction
    {
        public override Localization PhaseNameLocalization => new("PhaseName_RestorePhase");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_RestorePhase");
        public override Phase MainPhase => Phase.Restore;
        //public override bool CanUndo => false;

        public State Discarding { get; private set; }

        /*******************************************************************/
        public DiscardMaxHandSizeGameAction SetWith(Investigator investigator)
        {
            ActiveInvestigator = investigator;
            Discarding = new State(false);
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Discarding, true).Execute();
            await _gameActionsProvider.Create<SafeWhile>().SetWith(() => Discarding.IsActive && ActiveInvestigator.HandSize > ActiveInvestigator.MaxHandSize.Value, Discard).Execute();

            /*******************************************************************/
            async Task Discard()
            {
                await _gameActionsProvider.Create<CheckMaxHandSizeGameAction>().SetWith(ActiveInvestigator).Execute();
            }

            //await _gameActionsProvider.Create<CheckMaxHandSizeGameAction>().SetWith(ActiveInvestigator).Execute();
        }
    }

}
