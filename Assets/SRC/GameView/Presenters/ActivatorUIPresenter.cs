using MythsAndHorrors.GameRules;
using System;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ActivatorUIPresenter : IUIActivator
    {
        private List<Card> _cards = new();
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public void Activate() => Activate(_cards);

        public void Activate(List<Card> cards)
        {
            _cards = cards ?? throw new ArgumentNullException(null, "Cards is null");
            _ioActivatorComponent.ActivateSensor();

            _cards.ForEach(card => _cardViewsManager.Get(card).GlowView.SetGreenGlow());
            _avatarViewsManager.AvatarsPlayabled(_cards).ForEach(avatar => avatar.ActivateGlow());
        }

        public void Deactivate()
        {
            _ioActivatorComponent.DeactivateSensor();
            _cards?.ForEach(card => _cardViewsManager.Get(card).GlowView.Off());
        }
    }
}
