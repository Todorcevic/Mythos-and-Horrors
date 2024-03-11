using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ActivatePlayablesHandler
    {
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly List<IPlayable> _allPlayablesComponent;

        /*******************************************************************/
        public void ActiavatePlayables(List<IPlayable> specificsCardViews = null)
        {
            CheckActivesActivables(specificsCardViews);
            CheckActivesAvatars();
            CheckActivesIOActivator();

            /*******************************************************************/
            void CheckActivesActivables(List<IPlayable> specificsCardViews)
            {
                _allPlayablesComponent.Concat(specificsCardViews ?? _cardViewsManager.GetAllIPlayable())
                    .Where(playable => playable.CanBePlayed).ToList().ForEach(playable => playable.ActivateToClick());

                _cardViewsManager.AllCardsView.ForEach(cardView => cardView.AddBuffs());
            }

            void CheckActivesAvatars() => _avatarViewsManager.AvatarsPlayabled().ForEach(avatar => avatar.ActivateGlow());

            void CheckActivesIOActivator()
            {
                _ioActivatorComponent.ActivateCardSensors();
                _ioActivatorComponent.UnblockUI();
            }
        }

        public async Task DeactivatePlayables(List<IPlayable> clones = null)
        {
            CheckDeactivateActivables();
            CheckDeactivateAvatars();
            await CheckDeactivateIOActivator();

            /*******************************************************************/
            void CheckDeactivateActivables()
            {
                _allPlayablesComponent.Concat(clones ?? _cardViewsManager.GetAllIPlayable())
                    .Where(playable => playable.CanBePlayed).ToList().ForEach(playable => playable.DeactivateToClick());
                _cardViewsManager.AllCardsView.ForEach(cardView => cardView.RemoveBuffs());
            }

            void CheckDeactivateAvatars() => _avatarViewsManager.AvatarsPlayabled().ForEach(avatar => avatar.DeactivateGlow());

            async Task CheckDeactivateIOActivator()
            {
                await _ioActivatorComponent.DeactivateCardSensors();
                _ioActivatorComponent.BlockUI();
            }
        }
    }
}
