using System.Collections.Generic;
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
            CheckActivateCards();
            CheckActivateAvatars();
            CheckActivateTokensPile();
            CheckActivateMainButton();
            CheckActivateIOActivator();

            /*******************************************************************/
            void CheckActivateCards()
            {
                List<CardView> activablesCardViews = specificsCardViews ?? _cardViewsManager.GetAllCanPlay();
                activablesCardViews.ForEach(cardView => cardView.ActivateToClick());
            }

            void CheckActivateAvatars() => _avatarViewsManager.AvatarsPlayabled().ForEach(avatar => avatar.ActivateGlow());

            void CheckActivateTokensPile() => _tokensPileComponent.ActivateToClick();

            void CheckActivateMainButton()
            {
                if (withMainButton) _mainButtonComponent.Activate(_textsManager.ViewText.BUTTON_DONE);
            }

            void CheckActivateIOActivator()
            {
                _ioActivatorComponent.ActivateCardSensors();
                _ioActivatorComponent.UnblockUI();
            }
        }

        public async Task DeactivatePlayables()
        {
            CheckDeactivateCards();
            CheckDeactivateAvatars();
            CheckDeactivateTokensPile();
            CheckDeactivateMainButton();
            await CheckDeactivateIOActivator();

            /*******************************************************************/
            void CheckDeactivateCards()
            {
                _cardViewsManager.AllCardsView?.ForEach(card => card.DeactivateToClick());
            }

            void CheckDeactivateAvatars() => _avatarViewsManager.AvatarsPlayabled().ForEach(avatar => avatar.DeactivateGlow());

            void CheckDeactivateTokensPile() => _tokensPileComponent.DeactivateToClick();

            void CheckDeactivateMainButton() => _mainButtonComponent.Deactivate();

            async Task CheckDeactivateIOActivator()
            {
                await _ioActivatorComponent.DeactivateCardSensors();
                _ioActivatorComponent.BlockUI();
            }
        }
    }
}
