using System.Threading.Tasks;

namespace MythosAndHorrors.GameView
{
    public class ClickHandler
    {
        private TaskCompletionSource<IPlayable> waitForSelection = new();

        /*******************************************************************/
        public Task<IPlayable> WaitingClick()
        {
            return waitForSelection.Task;
        }

        public void Clicked(IPlayable element)
        {
            waitForSelection.SetResult(element);
            waitForSelection = new();
        }
    }
}
