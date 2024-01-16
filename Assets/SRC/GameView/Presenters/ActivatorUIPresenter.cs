using MythsAndHorrors.GameRules;
using System;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ActivatorUIPresenter : IUIActivable
    {
        private List<Card> _cards = new();
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public void DeactivateAll()
        {
            if (_ioActivatorComponent.IsSensorActivated)
                HideCardsPlayables();
            _ioActivatorComponent.DeactivateSensor();
            _ioActivatorComponent.DeactivateUI();
        }

        public void ActivateAll(List<Card> cards)
        {
            _cards = cards ?? throw new ArgumentNullException(null, "Cards is null");
            _ioActivatorComponent.ActivateSensor();
            _ioActivatorComponent.ActivateUI();
            ShowCardsPlayables();
        }

        public void ActivateSensor()
        {
            _ioActivatorComponent.ActivateSensor();
        }

        public void DeactivateSensor()
        {
            _ioActivatorComponent.DeactivateSensor();
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
