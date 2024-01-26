using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InteractablePresenter : IPresenter, IInteractable
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly CardViewGeneratorComponent _cardViewGeneratorComponent;
        [Inject] private readonly ShowSelectorComponent _showCenterComponent;

        private TaskCompletionSource<CardView> waitForSelection;

        /*******************************************************************/
        public async Task<Effect> Interact(InteractableGameAction interactableGameAction)
        {
            waitForSelection = new();
            List<CardView> allCardViews = _cardViewsManager.Get(interactableGameAction.ActivableCards);
            PreInteraction();
            CardView cardViewChoose = await waitForSelection.Task;
            await PostInteraction();
            return await ResolveEffectFrom(cardViewChoose);

            void PreInteraction()
            {
                ActivateInteraction(!interactableGameAction.IsManadatary);
                ShowCardsPlayables(allCardViews);
            }

            async Task PostInteraction()
            {
                HideCardsPlayables(allCardViews);
                await DeactivateInteraction();
                await _showCenterComponent.ReturnPlayables(cardViewChoose);
            }

            async Task<Effect> ResolveEffectFrom(CardView cardViewChoose) => cardViewChoose?.Card.HasMultiEffect ?? false
                    ? await ShowMultiEffects(cardViewChoose.Card.PlayableEffects.ToList()) ?? await Interact(interactableGameAction)
                    : cardViewChoose?.Card.PlayableEffects.FirstOrDefault();
        }

        void IInteractable.Clicked(CardView cardView) => waitForSelection.SetResult(cardView);

        private async Task<Effect> ShowMultiEffects(List<Effect> effects)
        {
            waitForSelection = new();
            Dictionary<CardView, Effect> clonesCardViewDictionary = CreateCardViewDictionary();
            await PreInteraction();
            CardView cardViewSelected = await waitForSelection.Task;
            Effect effectSelected = cardViewSelected == null ? null : clonesCardViewDictionary[cardViewSelected]; //Before destroy clones
            await PostInteraction(cardViewSelected);
            return effectSelected;

            /*******************************************************************/
            Dictionary<CardView, Effect> CreateCardViewDictionary()
            {
                CardView originalCardView = _cardViewsManager.Get(effects.First().Card);
                Dictionary<CardView, Effect> newClonesCardView = new() { { originalCardView, effects.First() } };
                foreach (Effect effect in effects.Skip(1))
                {
                    CardView cloneCardView = _cardViewGeneratorComponent.Clone(originalCardView, originalCardView.CurrentZoneView.transform);
                    newClonesCardView.Add(cloneCardView, effect);
                }
                return newClonesCardView;
            }

            async Task PreInteraction()
            {
                await _showCenterComponent.ShowMultiEffects(clonesCardViewDictionary).AsyncWaitForCompletion();
                _mainButtonComponent.Activate();
                _ioActivatorComponent.ActivateSensor();
                ShowCardsPlayables(clonesCardViewDictionary.Keys.ToList());
            }

            async Task PostInteraction(CardView cardViewSelected)
            {
                HideCardsPlayables(clonesCardViewDictionary.Keys.ToList());
                await DeactivateInteraction();
                if (cardViewSelected == null) await _showCenterComponent.ReturnClones();
                else await _showCenterComponent.DestroyClones(cardViewSelected);
            }
        }

        private void ActivateInteraction(bool withButton)
        {
            if (withButton) _mainButtonComponent.Activate();
            _ioActivatorComponent.ActivateSensor();
            _ioActivatorComponent.ActivateUI();
        }

        private async Task DeactivateInteraction()
        {
            _mainButtonComponent.Deactivate();
            if (_ioActivatorComponent.IsSensorActivated) await _ioActivatorComponent.DeactivateSensor();
            if (_ioActivatorComponent.IsUIActivated) _ioActivatorComponent.DeactivateUI();
        }

        private void ShowCardsPlayables(List<CardView> _cards)
        {
            _cards.ForEach(card => card.ActivateToClick());
            _avatarViewsManager.AvatarsPlayabled(_cards.Select(cardView => cardView.Card).ToList()).ForEach(avatar => avatar.ActivateGlow());
        }

        private void HideCardsPlayables(List<CardView> _cards)
        {
            _cards?.ForEach(card => card.DeactivateToClick());
            _avatarViewsManager.AvatarsPlayabled(_cards.Select(cardView => cardView.Card).ToList()).ForEach(avatar => avatar.DeactivateGlow());
        }
    }
}
