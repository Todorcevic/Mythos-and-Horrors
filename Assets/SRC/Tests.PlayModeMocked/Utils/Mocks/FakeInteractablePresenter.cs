using MythosAndHorrors.GameRules;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;


namespace MythosAndHorrors.PlayMode.Tests
{
    public class FakeInteractablePresenter : IInteractablePresenter
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        private TaskCompletionSource<Effect> waitForClicked = new();

        /*******************************************************************/
        public void ClickedUndoButton()
        {
            if (_gameActionsProvider.CurrentInteractable.UndoEffect == null)
                throw new InvalidOperationException("UndoEffect is null");
            waitForClicked.SetResult(_gameActionsProvider.CurrentInteractable.UndoEffect);
        }

        public void ClickedMainButton()
        {
            if (_gameActionsProvider.CurrentInteractable.MainButtonEffect == null)
                throw new InvalidOperationException("MainButtonEffect is null");
            waitForClicked.SetResult(_gameActionsProvider.CurrentInteractable.MainButtonEffect);
        }

        public void ClickedTokenButton()
        {
            if (_gameActionsProvider.CurrentInteractable is not OneInvestigatorTurnGameAction oneTurnGameAction)
                throw new InvalidOperationException("Not interactableGameAction");
            if (oneTurnGameAction.TakeResourceEffect == null)
                throw new InvalidOperationException("TakeResourceEffect is null");
            waitForClicked.SetResult(oneTurnGameAction.TakeResourceEffect);
        }

        public void ClickedIn(Card cardSelected)
        {
            Effect effect = cardSelected.PlayableEffects.FirstOrDefault() ?? throw new InvalidOperationException($"Card {cardSelected.Info.Code} not has Effect");
            waitForClicked.SetResult(effect);
        }

        public void ClickedIn(Effect effectSelected)
        {
            if (effectSelected == null)
                throw new InvalidOperationException("Effect is null");
            waitForClicked.SetResult(effectSelected);
        }

        public async Task<Effect> SelectWith(GameAction gamAction)
        {
            await Task.WhenAny(waitForClicked.Task, Task.Delay(100));

            if (!waitForClicked.Task.IsCompleted)
                throw new TimeoutException("The operation has exceeded. Timeout.");

            Effect effectToSend = waitForClicked.Task.Result;
            waitForClicked = new TaskCompletionSource<Effect>();
            return effectToSend;
        }
    }
}