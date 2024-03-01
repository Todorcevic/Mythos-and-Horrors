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
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly List<IPlayable> _allPlayablesComponent;

        private List<IPlayable> AllIPlayablesActivables => _allPlayablesComponent.Concat(_cardViewsManager.GetAllIPlayable())
            .Where(playable => playable.CanBePlayed).ToList();

        /*******************************************************************/
        public void ActiavatePlayables(List<IPlayable> specificsCardViews = null)
        {
            CheckActivateActivables(specificsCardViews);
            CheckActivateAvatars();
            CheckActivateIOActivator();

            /*******************************************************************/
            void CheckActivateActivables(List<IPlayable> specificsCardViews)
            {
                List<IPlayable> activablesCardViews = specificsCardViews ?? _cardViewsManager.GetAllIPlayable();
                activablesCardViews.Concat(_allPlayablesComponent).Where(playable => playable.CanBePlayed).ToList().ForEach(playable => playable.ActivateToClick());
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
