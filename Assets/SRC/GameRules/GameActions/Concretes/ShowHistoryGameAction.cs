using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class ShowHistoryGameAction : GameAction
    {
        private readonly TaskCompletionSource<bool> waitForContinue = new();
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
            await waitForContinue.Task;
        }

        public void Continue()
        {
            waitForContinue.SetResult(true);
        }
    }
}
