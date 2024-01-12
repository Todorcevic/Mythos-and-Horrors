using MythsAndHorrors.GameRules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ActivatorUIPresenter : IAnimator
    {
        private List<Card> _cards = new();
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public async Task Checking(GameAction gameAction)
        {
            if (gameAction is InteractableGameAction waitingForSelectionGameAction)
                ActivateAll(waitingForSelectionGameAction.ActivableCards);
            else DeactivateAll();

            await Task.CompletedTask;
        }

        /*******************************************************************/
        public void Activate()
        {
            _ioActivatorComponent.ActivateSensor();
            _ioActivatorComponent.ActivateUI();
        }

        public void Deactivate()
        {
            _ioActivatorComponent.DeactivateSensor();
            _ioActivatorComponent.DeactivateUI();
        }

        public void ActivateUI()
        {
            _ioActivatorComponent.ActivateUI();
        }

        public void DeactivateAll()
        {
            if (!_ioActivatorComponent.IsFullyActivated) return;
            Deactivate();
            HideCardsPlayables();
        }

        private void ActivateAll(List<Card> cards)
        {
            _cards = cards ?? throw new ArgumentNullException(null, "Cards is null");
            Activate();
            ShowCardsPlayables();
        }

        private void ShowCardsPlayables()
        {
            _cards.ForEach(card => _cardViewsManager.Get(card).GlowView.SetGreenGlow());
            _avatarViewsManager.AvatarsPlayabled(_cards).ForEach(avatar => avatar.ActivateGlow());
        }

        private void HideCardsPlayables()
        {
            _cards?.ForEach(card => _cardViewsManager.Get(card).GlowView.Off());
            _avatarViewsManager.AvatarsPlayabled(_cards).ForEach(avatar => avatar.DeactivateGlow());
        }
    }
}
