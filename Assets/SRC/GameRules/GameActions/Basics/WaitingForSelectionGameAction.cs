using System.Threading.Tasks;

namespace GameRules
{
    public class WaitingForSelectionGameAction : GameAction
    {
        private static TaskCompletionSource<bool> waitForSelection;

        /*******************************************************************/
        public async Task Run() => await Start();

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            waitForSelection = new();
            await waitForSelection.Task;
        }

        public static void Clicked()
        {
            waitForSelection.SetResult(true);
        }
    }
}
