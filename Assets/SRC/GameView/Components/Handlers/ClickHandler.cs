using System.Threading.Tasks;

namespace MythosAndHorrors.GameView
{
    public class ClickHandler<T>
    {
        private TaskCompletionSource<T> waitForSelection = new();

        /*******************************************************************/
        public Task<T> WaitingClick() => waitForSelection.Task;

        public void Clicked(T element)
        {
            waitForSelection.SetResult(element);
            waitForSelection = new();
        }
    }
}
