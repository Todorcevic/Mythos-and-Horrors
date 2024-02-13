using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ActivateCardViewsHandler
    {
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly ViewText _gameText;

        /*******************************************************************/
        public void ActiavateCardViewsPlayables(List<CardView> _cards, bool withMainButton)
        {
            _cards.ForEach(card => card.ActivateToClick());
            _avatarViewsManager.AvatarsPlayabled(_cards.Select(cardView => cardView.Card).ToList()).ForEach(avatar => avatar.ActivateGlow());

            if (withMainButton) _mainButtonComponent.Activate(_gameText.BUTTON_DONE);
            _ioActivatorComponent.ActivateCardSensors();
            _ioActivatorComponent.UnblockUI();
        }

        public async Task DeactivateCardViewsPlayables(List<CardView> _cards)
        {
            _cards?.ForEach(card => card.DeactivateToClick());
            _avatarViewsManager.AvatarsPlayabled(_cards.Select(cardView => cardView.Card).ToList()).ForEach(avatar => avatar.DeactivateGlow());

            _mainButtonComponent.Deactivate();
            if (_ioActivatorComponent.IsSensorActivated) await _ioActivatorComponent.DeactivateCardSensors();
            if (_ioActivatorComponent.IsUIActivated) _ioActivatorComponent.BlockUI();
        }
    }
}
