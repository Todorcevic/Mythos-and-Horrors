using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InteractablePresenter : IInteractableAnimator
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;

        private TaskCompletionSource<bool> waitForSelection;
        private Card cardSelected;

        /*******************************************************************/
        public async Task<Card> Interact(InteractableGameAction interactableGameAction)
        {
            waitForSelection = new();
            Activate(interactableGameAction.ActivableCards);
            await waitForSelection.Task;
            await Deactivate(interactableGameAction.ActivableCards);
            return cardSelected;
        }

        public void Clicked(Card card = null)
        {
            cardSelected = card;
            waitForSelection.SetResult(true);
        }

        private void Activate(List<Card> _cards)
        {
            _ioActivatorComponent.ActivateSensor();
            _ioActivatorComponent.ActivateUI();
            ShowCardsPlayables(_cards);
        }

        private async Task Deactivate(List<Card> _cards)
        {
            HideCardsPlayables(_cards);
            if (_ioActivatorComponent.IsSensorActivated) await _ioActivatorComponent.DeactivateSensor();
            if (_ioActivatorComponent.IsUIActivated) _ioActivatorComponent.DeactivateUI();
        }

        private void ShowCardsPlayables(List<Card> _cards)
        {
            _cards.ForEach(card => _cardViewsManager.Get(card).GlowView.SetGreenGlow());
            _avatarViewsManager.AvatarsPlayabled(_cards).ForEach(avatar => avatar.ActivateGlow());
        }

        private void HideCardsPlayables(List<Card> _cards)
        {
            _cards?.ForEach(card => _cardViewsManager.Get(card).GlowView.Off());
            _avatarViewsManager.AvatarsPlayabled(_cards).ForEach(avatar => avatar.DeactivateGlow());
        }
    }
}
