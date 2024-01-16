using MythsAndHorrors.GameRules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ActivatorInteractionPresenter : IInteractable
    {
        private List<Card> _cards = new();
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public async Task DeactivateAll()
        {
            if (_ioActivatorComponent.IsSensorActivated)
            {
                HideCardsPlayables();
                await DeactivateSensor();
            }
            if (_ioActivatorComponent.IsUIActivated) DeactivateUI();
        }

        public void ActivateAll(List<Card> cards)
        {
            _cards = cards ?? throw new ArgumentNullException(null, "Cards is null");
            ActivateSensor();
            ActivateUI();
            ShowCardsPlayables();
        }

        public void ActivateSensor()
        {
            _ioActivatorComponent.ActivateSensor();
        }

        public async Task DeactivateSensor()
        {
            await _ioActivatorComponent.DeactivateSensor();
        }

        public void ActivateUI()
        {
            _ioActivatorComponent.ActivateUI();
        }

        public void DeactivateUI()
        {
            _ioActivatorComponent.DeactivateUI();
        }

        private void ShowCardsPlayables()
        {
            _cards.ForEach(card => _cardViewsManager.Get(card).GlowView.SetGreenGlow());
            _avatarViewsManager.AvatarsPlayabled(_cards).ForEach(avatar => avatar.ActivateGlow());
        }

        private void HideCardsPlayables()
        {
            _cards?.ForEach(card => _cardViewsManager.Get(card).GlowView.Off());
            _avatarViewsManager.AvatarsPlayabled(_cards).ForEach(avatar => avatar.DeactivateGlow());
        }
    }
}
