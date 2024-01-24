using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InteractablePresenter : IPresenter, IInteractable
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly MainButtonComponent _buttonController;
        [Inject] private readonly CardViewGeneratorComponent _cardViewGeneratorComponent;
        [Inject] private readonly ShowSelectorComponent _showCenterComponent;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        private TaskCompletionSource<CardView> waitForSelection;

        /*******************************************************************/
        public async Task<Card> Interact(InteractableGameAction interactableGameAction)
        {
            waitForSelection = new();
            List<CardView> allCardViews = _cardViewsManager.Get(interactableGameAction.ActivableCards);
            Activate(!interactableGameAction.IsManadatary);
            ShowCardsPlayables(allCardViews);

            Card choose = await ResolveCardSelected(await waitForSelection.Task);

            HideCardsPlayables(allCardViews);
            return choose;

            async Task<Card> ResolveCardSelected(CardView cardViewSelected)
            {
                await Deactivate();
                if (cardViewSelected == null) return null;
                else if (cardViewSelected.Card.HasMultiEffect) return await ShowMultiEffects(cardViewSelected.Card.PlayableEffects.ToList(), interactableGameAction);
                else return cardViewSelected.Card;
            }
        }

        void IInteractable.Clicked(CardView cardView) => waitForSelection.SetResult(cardView);

        private async Task<Card> ShowMultiEffects(List<Effect> effects, InteractableGameAction interactableGameAction)
        {
            waitForSelection = new();
            CardView originalCardView = _cardViewsManager.Get(effects[0].Card);
            Dictionary<CardView, Effect> clonesCardView = CreateCardViewDictionary();
            await PreResolve();

            CardView cardViewSelected = await waitForSelection.Task;

            Card cardSelected = ResolveCardSelected(cardViewSelected);
            await PostResolve(cardViewSelected);
            return cardSelected ?? await Interact(interactableGameAction);

            /*******************************************************************/
            Dictionary<CardView, Effect> CreateCardViewDictionary()
            {
                Dictionary<CardView, Effect> newClonesCardView = new() { { originalCardView, effects.First() } };
                foreach (Effect effect in effects.Skip(1))
                {
                    CardView cloneCardView = _cardViewGeneratorComponent.Clone(originalCardView, originalCardView.CurrentZoneView.transform);
                    newClonesCardView.Add(cloneCardView, effect);
                }
                return newClonesCardView;
            }

            async Task PreResolve()
            {
                await _showCenterComponent.ShowCenter(clonesCardView).AsyncWaitForCompletion();
                Activate(withButton: true);
                ShowCardsPlayables(clonesCardView.Keys.ToList());
            }

            Card ResolveCardSelected(CardView cardViewSelected)
            {
                if (cardViewSelected == null) return null;
                Effect effectSelected = clonesCardView[cardViewSelected];
                effectSelected.Card.ClearEffects();
                effectSelected.Card.AddEffect(effectSelected);
                return effectSelected.Card;
            }

            async Task PostResolve(CardView cardViewSelected)
            {
                await Deactivate();
                _showCenterComponent.Shutdown();
                List<CardView> clones = clonesCardView.Keys.Except(new List<CardView> { originalCardView }).ToList();
                if (cardViewSelected == null) await ReturnClones(clones);
                else await DestroyClones(clones, cardViewSelected);
            }

            async Task ReturnClones(List<CardView> clones)
            {
                clones.ForEach(clone => clone.MoveToZone(_zoneViewsManager.CenterShowZone, Ease.OutSine)
                    .OnComplete(() => Object.Destroy(clone.gameObject)));
                await _moveCardHandler.MoveCardWithPreviewToZone(originalCardView, _zoneViewsManager.Get(originalCardView.Card.CurrentZone));
            }

            async Task DestroyClones(List<CardView> clones, CardView cardViewSelected)
            {
                (originalCardView.transform.position, cardViewSelected.transform.position) = (cardViewSelected.transform.position, originalCardView.transform.position);

                await _moveCardHandler.MoveCardsToZone(clones, _zoneViewsManager.OutZone);
                clones.ForEach(cardView => Object.Destroy(cardView.gameObject));
            }
        }

        private void Activate(bool withButton)
        {
            if (withButton) _buttonController.Activate();
            _ioActivatorComponent.ActivateSensor();
            _ioActivatorComponent.ActivateUI();
        }

        private async Task Deactivate()
        {
            _buttonController.Deactivate();
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
