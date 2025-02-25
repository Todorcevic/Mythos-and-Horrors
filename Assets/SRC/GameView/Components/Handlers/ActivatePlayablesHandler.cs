using Sirenix.Utilities;
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

        public void ActivatePlayables()
        {
            ShowCardsState();
            CheckActivesActivables();
            CheckActivesAvatars();
            _ioActivatorComponent.ActivateCardSensors();

            /*******************************************************************/
            void CheckActivesActivables()
            {
                _allPlayablesComponent.Concat(_cardViewsManager.GetAllIPlayable())
                    .Where(playable => playable.CanBePlayed)
                    .ForEach(playable => playable.ActivateToClick());
                _cardViewsManager.AllCardsView.ForEach(cardView => cardView.AddBuffs());
            }

            void CheckActivesAvatars() => _avatarViewsManager.AvatarsPlayabled(_cardViewsManager.GetAllIPlayable())
                .ForEach(avatar => avatar.ActivateGlow());
        }

        public void ActivateClones(List<IPlayable> clones)
        {
            ShowCardsState();
            CheckActivesActivables();
            CheckActivesAvatars();
            _ioActivatorComponent.ActivateCardSensors();

            /*******************************************************************/
            void CheckActivesActivables()
            {
                clones.Where(playable => playable.CanBePlayed)
                    .ForEach(playable => playable.ActivateToClick());
                _cardViewsManager.AllCardsView.ForEach(cardView => cardView.AddBuffs());
            }

            void CheckActivesAvatars() => _avatarViewsManager.AvatarsPlayabled(clones)
                .ForEach(avatar => avatar.ActivateGlow());
        }

        public async Task DeactivatePlayables()
        {
            HideCardsState();
            CheckDeactivateActivables();
            CheckDeactivateAvatars();
            _ioActivatorComponent.DeactivateCardSensors();
            await DotweenExtension.WaitForAnimationsComplete();

            /*******************************************************************/
            void CheckDeactivateActivables()
            {
                _allPlayablesComponent.Concat(_cardViewsManager.GetAllIPlayable()).Where(playable => playable.CanBePlayed)
                    .ForEach(playable => playable.DeactivateToClick());

                _cardViewsManager.AllCardsView.ForEach(cardView => cardView.RemoveBuffs());
            }

            void CheckDeactivateAvatars() =>
                _avatarViewsManager.AvatarsPlayabled(_cardViewsManager.GetAllIPlayable()).ForEach(avatar => avatar.DeactivateGlow());
        }

        public async Task DeactivateClones(List<IPlayable> clones)
        {
            HideCardsState();
            CheckDeactivateActivables();
            CheckDeactivateAvatars();
            _ioActivatorComponent.DeactivateCardSensors();
            await DotweenExtension.WaitForAnimationsComplete();

            /*******************************************************************/
            void CheckDeactivateActivables()
            {
                clones.Where(playable => playable.CanBePlayed).ForEach(playable => playable.DeactivateToClick());
                _cardViewsManager.AllCardsView.ForEach(cardView => cardView.RemoveBuffs());
            }

            void CheckDeactivateAvatars() => _avatarViewsManager.AvatarsPlayabled(clones)
                .ForEach(avatar => avatar.DeactivateGlow());
        }
    }
}
