using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ActivatePlayablesHandler
    {
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly TokensPileComponent _tokensPileComponent;
        [Inject] private readonly TextsManager _textsManager;
        [Inject] private readonly CardViewsManager _cardViewsManager;

        /*******************************************************************/
        public void ActiavatePlayables(bool withMainButton, List<CardView> specificsCardViews = null)
        {
            List<CardView> activablesCardViews = specificsCardViews ?? _cardViewsManager.AllCardsView.Where(cardView => cardView.Card.CanPlay).ToList();

            activablesCardViews.ForEach(cardView => cardView.AddBuffsAndEffects());
            activablesCardViews.ForEach(cardView => cardView.ActivateToClick());

            _avatarViewsManager.AvatarsPlayabled(activablesCardViews.Select(cardView => cardView.Card).ToList()).ForEach(avatar => avatar.ActivateGlow());

            if (_tokensPileComponent.CanPlayResource) _tokensPileComponent.ActivateToClick();
            if (withMainButton) _mainButtonComponent.Activate(_textsManager.ViewText.BUTTON_DONE);

            _ioActivatorComponent.ActivateCardSensors();
            _ioActivatorComponent.UnblockUI();
        }

        public async Task DeactivatePlayables()
        {
            _cardViewsManager.AllCardsView?.ForEach(card => card.RemoveBuffsAndEffects());
            _cardViewsManager.AllCardsView?.ForEach(card => card.DeactivateToClick());

            _avatarViewsManager.AvatarsPlayabled(_cardViewsManager.AllCardsView.Select(cardView => cardView.Card).ToList()).ForEach(avatar => avatar.DeactivateGlow());

            _tokensPileComponent.DeactivateToClick();
            _mainButtonComponent.Deactivate();

            await _ioActivatorComponent.DeactivateCardSensors();
            _ioActivatorComponent.BlockUI();
        }
    }
}
