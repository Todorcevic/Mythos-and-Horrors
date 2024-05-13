using MythosAndHorrors.GameRules;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class FakeInteractablePresenter : IInteractablePresenter
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        private TaskCompletionSource<Effect> waitForClicked = new();

        /*******************************************************************/
        public void ClickUndoButton()
        {
            waitForClicked.SetResult(_gameActionsProvider.CurrentInteractable.UndoEffect);
        }

        public void ClickMainButton()
        {
            waitForClicked.SetResult(_gameActionsProvider.CurrentInteractable.MainButtonEffect);
        }

        public void ClickedIn(Card cardSelected)
        {
            waitForClicked.SetResult(cardSelected.PlayableEffects.First());
        }

        public void ClickedIn(Effect effectSelected)
        {
            waitForClicked.SetResult(effectSelected);
        }

        public async Task<Effect> SelectWith(GameAction gamAction)
        {
            await Task.WhenAny(waitForClicked.Task, Task.Delay(1000));

            if (!waitForClicked.Task.IsCompleted)
                throw new TimeoutException("The operation has exceeded. Timeout.");

            Effect effectToSend = waitForClicked.Task.Result;
            waitForClicked = new TaskCompletionSource<Effect>();
            return effectToSend;
        }
    }
}