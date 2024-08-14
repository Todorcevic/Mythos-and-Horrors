using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class RunAwayGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public override bool CanBeExecuted => Investigator.IsInPlay;

        /*******************************************************************/
        public RunAwayGameAction SetWith(Investigator investigator)
        {
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<EliminateInvestigatorGameAction>().SetWith(Investigator).Execute();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Investigator.Resign, IsActive).Execute();
        }
    }
}
