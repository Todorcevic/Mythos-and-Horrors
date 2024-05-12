using MythosAndHorrors.GameRules;
using System.Threading.Tasks;

namespace MythosAndHorrors.EditMode.Tests
{
    public class FakeInteractablePresenter : IInteractablePresenter
    {
        private TaskCompletionSource<Effect> waitForClicked = new();

        public void ClickedIn(Effect effectSelected)
        {
            waitForClicked.SetResult(effectSelected);
        }

        public async Task<Effect> SelectWith(GameAction gamAction)
        {
            await waitForClicked.Task;
            Effect effectToSend = waitForClicked.Task.Result;
            waitForClicked = new();
            return effectToSend;
        }
    }
}