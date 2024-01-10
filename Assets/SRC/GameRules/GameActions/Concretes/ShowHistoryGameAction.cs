using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class ShowHistoryGameAction : GameAction
    {
        private readonly TaskCompletionSource<bool> waitForSelection = new();
        public History History { get; private set; }

        /*******************************************************************/
        public async Task Run(History history)
        {
            History = history;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await waitForSelection.Task;
        }

        public void Continue()
        {
            waitForSelection.SetResult(true);
        }
    }
}
