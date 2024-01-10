using MythsAndHorrors.GameRules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ActivatorUIPresenter : IAnimatorStart
    {
        private bool isDeactivated = false;
        private List<Card> _cards = new();
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public async Task CheckingAtStart(GameAction gameAction)
        {
            if (gameAction is InteractableGameAction waitingForSelectionGameAction)
                ActivateCards(waitingForSelectionGameAction.ActivableCards);
            else Deactivate();

            await Task.CompletedTask;
        }

        /*******************************************************************/
        public void Activate() => ActivateCards(_cards);

        public void ActivateUI()
        {
            isDeactivated = false;
            _ioActivatorComponent.ActivateUI();
        }

        public void Deactivate()
        {
            if (isDeactivated) return;
            isDeactivated = true;
            _ioActivatorComponent.DeactivateSensor();
            _ioActivatorComponent.DeactivateUI();
            _cards?.ForEach(card => _cardViewsManager.Get(card).GlowView.Off());
            _avatarViewsManager.AvatarsPlayabled(_cards).ForEach(avatar => avatar.DeactivateGlow());
        }

        private void ActivateCards(List<Card> cards)
        {
            isDeactivated = false;
            _cards = cards ?? throw new ArgumentNullException(null, "Cards is null");
            _ioActivatorComponent.ActivateSensor();
            _ioActivatorComponent.ActivateUI();

            _cards.ForEach(card => _cardViewsManager.Get(card).GlowView.SetGreenGlow());
            _avatarViewsManager.AvatarsPlayabled(_cards).ForEach(avatar => avatar.ActivateGlow());
        }


    }
}
