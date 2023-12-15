using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ActivatorUIPresenter : IUIActivator
    {
        private List<Card> _cards;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public void Activate() => Activate(_cards);

        public void Activate(List<Card> cards)
        {
            _cards = cards;
            _ioActivatorComponent.ActivateSensor();

            cards?.ForEach(card => _cardViewsManager.Get(card).GlowView.SetGreenGlow());
            _avatarViewsManager.AvatarsPlayabled(cards)?.ForEach(avatar => avatar.ActivateGlow());
        }

        public void Deactivate()
        {
            _ioActivatorComponent.DeactivateSensor();
            _cards?.ForEach(card => _cardViewsManager.Get(card).GlowView.Off());
        }
    }
}
