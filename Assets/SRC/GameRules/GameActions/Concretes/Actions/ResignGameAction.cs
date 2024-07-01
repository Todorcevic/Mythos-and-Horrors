using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ResignGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public ResignGameAction SetWith(Investigator investigator)
        {
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new EliminateInvestigatorGameAction(Investigator));
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Investigator.Resign, IsActive).Start();
        }
    }
}
