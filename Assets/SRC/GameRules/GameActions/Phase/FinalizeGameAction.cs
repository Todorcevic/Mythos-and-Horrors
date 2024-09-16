using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class FinalizeGameAction : GameAction
    {
        public Resolution Resolution { get; private set; }

        /*******************************************************************/
        public FinalizeGameAction SetWith(Resolution resolution)
        {
            Resolution = resolution;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(Resolution.History).Execute();
            await Resolution.Logic.Invoke();
        }
    }
}
