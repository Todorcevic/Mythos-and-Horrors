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
        [Inject] private readonly List<IPlayable> _allPlayablesComponent; //Buttons

        /*******************************************************************/
        private void ShowCardsState() => _cardViewsManager.GetAllUpdatable().ForEach(deckCardView => deckCardView.Show());
        private void HideCardsState() => _cardViewsManager.GetAllUpdatable().ForEach(deckCardView => deckCardView.Hide());

        public void ActiavatePlayables(List<IPlayable> specificsCardViews = null)
        {
            ShowCardsState();
            CheckActivesActivables(specificsCardViews);
            CheckActivesAvatars(specificsCardViews);
            _ioActivatorComponent.ActivateCardSensors();

            /*******************************************************************/
            void CheckActivesActivables(List<IPlayable> specificsCardViews)
            {
                _allPlayablesComponent.Concat(specificsCardViews ?? _cardViewsManager.GetAllIPlayable())
                    .Where(playable => playable.CanBePlayed).ToList().ForEach(playable => playable.ActivateToClick());

                _cardViewsManager.AllCardsView.ForEach(cardView => cardView.AddBuffs());
            }

            void CheckActivesAvatars(List<IPlayable> specificsCardViews) => _avatarViewsManager
                .AvatarsPlayabled(specificsCardViews ?? _cardViewsManager.GetAllIPlayable()).ForEach(avatar => avatar.ActivateGlow());
        }

        public async Task DeactivatePlayables(List<IPlayable> clones = null)
        {
            HideCardsState();
            CheckDeactivateActivables();
            CheckDeactivateAvatars();
            await _ioActivatorComponent.DeactivateCardSensors();

            /*******************************************************************/
            void CheckDeactivateActivables()
            {
                _allPlayablesComponent.Concat(clones ?? _cardViewsManager.GetAllIPlayable())
                    .Where(playable => playable.CanBePlayed).ToList().ForEach(playable => playable.DeactivateToClick());
                _cardViewsManager.AllCardsView.ForEach(cardView => cardView.RemoveBuffs());
            }

            void CheckDeactivateAvatars() =>
                _avatarViewsManager.AvatarsPlayabled(clones ?? _cardViewsManager.GetAllIPlayable()).ForEach(avatar => avatar.DeactivateGlow());
        }

        public void ActivateMainButton()
        {
            _allPlayablesComponent.First(playable => playable is MainButtonComponent).ActivateToClick();
        }

        public void DeactivateMainButton()
        {
            _allPlayablesComponent.First(playable => playable is MainButtonComponent).DeactivateToClick();
        }
    }
}
