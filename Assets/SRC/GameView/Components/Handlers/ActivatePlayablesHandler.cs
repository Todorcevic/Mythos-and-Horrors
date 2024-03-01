using MythsAndHorrors.GameRules;
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
        [Inject] private readonly TextsManager _textsManager;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly List<IPlayable> _allPlayablesComponent;

        private List<IPlayable> AllIPlayablesActivables => _allPlayablesComponent.Concat(_cardViewsManager.GetAllIPlayable())
            .Where(playable => playable.CanBePlayed).ToList();

        /*******************************************************************/
        public void ActiavatePlayables(bool withMainButton, List<IPlayable> specificsCardViews = null)
        {
            if (withMainButton) _mainButtonComponent.SetButton(_textsManager.ViewText.BUTTON_DONE, new() { Effect.NullEffect });
            else _mainButtonComponent.Clear();

            CheckActivateActivables(specificsCardViews);
            CheckActivateAvatars();
            CheckActivateIOActivator();

            /*******************************************************************/
            void CheckActivateActivables(List<IPlayable> specificsCardViews)
            {
                List<IPlayable> activablesCardViews = specificsCardViews ?? AllIPlayablesActivables;
                activablesCardViews.ForEach(playable => playable.ActivateToClick());
            }

            void CheckActivateAvatars() => _avatarViewsManager.AvatarsPlayabled().ForEach(avatar => avatar.ActivateGlow());

            void CheckActivateIOActivator()
            {
                _ioActivatorComponent.ActivateCardSensors();
                _ioActivatorComponent.UnblockUI();
            }
        }

        public async Task DeactivatePlayables()
        {
            CheckDeactivateActivables();
            CheckDeactivateAvatars();
            await CheckDeactivateIOActivator();

            /*******************************************************************/

            void CheckDeactivateActivables() => AllIPlayablesActivables.ForEach(playable => playable.DeactivateToClick());

            void CheckDeactivateAvatars() => _avatarViewsManager.AvatarsPlayabled().ForEach(avatar => avatar.DeactivateGlow());

            async Task CheckDeactivateIOActivator()
            {
                await _ioActivatorComponent.DeactivateCardSensors();
                _ioActivatorComponent.BlockUI();
            }
        }
    }
}
