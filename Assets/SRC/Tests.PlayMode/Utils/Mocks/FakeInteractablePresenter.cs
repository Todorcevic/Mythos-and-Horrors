using MythosAndHorrors.GameRules;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zenject;
using System.Collections;
using UnityEngine;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class FakeInteractablePresenter : IInteractablePresenter
    {
        private const float TIMEOUT = 3f;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        private TaskCompletionSource<BaseEffect> waitForClicked = new();

        /*******************************************************************/
        public IEnumerator ClickedUndoButton()
        {
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !UndoButtonIsEnable()) yield return null;
            InteractableGameAction interactable = _gameActionsProvider.CurrentInteractable;
            BaseEffect effect = _gameActionsProvider.CurrentInteractable?.UndoEffect ??
                throw new InvalidOperationException("UndoEffect is null");
            waitForClicked.SetResult(effect);
            while (interactable == _gameActionsProvider.CurrentInteractable) yield return null;

            /*******************************************************************/
            bool UndoButtonIsEnable() => _gameActionsProvider.CurrentInteractable?.UndoEffect != null;
        }

        public IEnumerator ClickedMainButton()
        {
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !MainButtonIsEnable()) yield return null;
            InteractableGameAction interactable = _gameActionsProvider.CurrentInteractable;
            BaseEffect effect = _gameActionsProvider.CurrentInteractable?.MainButtonEffect ??
                throw new InvalidOperationException("MainButtonEffect is null");
            waitForClicked.SetResult(effect);
            while (interactable == _gameActionsProvider.CurrentInteractable && _gameActionsProvider.CurrentPayHintsToGoal == null)
            {
                yield return null;
            }

            /*******************************************************************/
            bool MainButtonIsEnable() => _gameActionsProvider.CurrentInteractable?.MainButtonEffect != null;
        }

        public IEnumerator ClickedTokenButton()
        {
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !TakeResourceIsEnable()) yield return null;
            InteractableGameAction interactable = _gameActionsProvider.CurrentInteractable;
            CardEffect effect = (_gameActionsProvider.CurrentInteractable as InvestigatorTurnGameAction)?.TakeResourceEffect ??
                throw new InvalidOperationException("TakeResourceEffect is null");
            waitForClicked.SetResult(effect);
            while (interactable == _gameActionsProvider.CurrentInteractable && _gameActionsProvider.CurrentPayHintsToGoal == null)
            {
                yield return null;
            }

            /*******************************************************************/
            bool TakeResourceIsEnable() => (_gameActionsProvider.CurrentInteractable as InvestigatorTurnGameAction)?.TakeResourceEffect != null;
        }

        public IEnumerator ClickedIn(Card cardSelected, int position = 0)
        {
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !AnyEffectInCard()) yield return null;
            InteractableGameAction interactable = _gameActionsProvider.CurrentInteractable;
            BaseEffect effect = cardSelected.PlayableEffects?.ElementAtOrDefault(position) ??
                throw new InvalidOperationException($"Card {cardSelected.Info.Code} not has Effect");
            waitForClicked.SetResult(effect);
            while (interactable == _gameActionsProvider.CurrentInteractable && _gameActionsProvider.CurrentPayHintsToGoal == null)
            {
                yield return null;
            }


            /*******************************************************************/
            bool AnyEffectInCard() => cardSelected.PlayableEffects?.Any() ?? false;
        }

        public bool IsClickable(Card cardSelected) => cardSelected.PlayableEffects?.Any() ?? false;

        /*******************************************************************/
        public async Task<BaseEffect> SelectWith(InteractableGameAction _)
        {
            await Task.WhenAny(waitForClicked.Task, Task.Delay(3000));

            if (!waitForClicked.Task.IsCompleted)
                throw new TimeoutException("The operation has exceeded. Timeout.");

            BaseEffect effectToSend = waitForClicked.Task.Result;
            waitForClicked = new TaskCompletionSource<BaseEffect>();
            return effectToSend;
        }
    }
}