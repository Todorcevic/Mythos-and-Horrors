using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InteractablePresenter : IPresenter
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly MainButtonComponent _buttonController;
        [Inject] private readonly CardViewGeneratorComponent _cardViewGeneratorComponent;
        [Inject] private readonly ShowCenterHandler _showCenterHandler;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        private Dictionary<CardView, Effect> clonesCardView;
        private TaskCompletionSource<bool> waitForSelection;
        private CardView _cardSelected;

        /*******************************************************************/
        public async Task<Card> Interact(InteractableGameAction interactableGameAction)
        {
            waitForSelection = new();
            List<CardView> allCardViews = _cardViewsManager.Get(interactableGameAction.ActivableCards);
            Activate(!interactableGameAction.IsManadatary);
            ShowCardsPlayables(allCardViews);
            await waitForSelection.Task;
            await Deactivate();
            HideCardsPlayables(allCardViews);
            if (_cardSelected == null) return null;
            Card choose = _cardSelected.Card.HasMultiEffect ?
                await ShowMultiEffects(_cardSelected.Card.PlayableEffects.ToList()) :
                _cardSelected.Card;
            if (choose == null) return await Interact(interactableGameAction);
            return choose;
        }

        private async Task<Card> ShowMultiEffects(List<Effect> effects)
        {
            waitForSelection = new();
            clonesCardView = new();
            CardView cardView = _cardViewsManager.Get(effects[0].Card);
            clonesCardView.Add(cardView, effects.First());

            foreach (Effect effect in effects.Skip(1))
            {
                CardView cloneCardView = _cardViewGeneratorComponent.Clone(cardView, cardView.CurrentZoneView.transform);
                clonesCardView.Add(cloneCardView, effect);
            }

            await _showCenterHandler.ShowCenter(clonesCardView.Keys.ToList()).AsyncWaitForCompletion();
            Activate(withButton: true);
            ShowCardsPlayables(clonesCardView.Keys.ToList());
            await waitForSelection.Task;
            await Deactivate();
            Card cardSelected = SelecteEffect();
            await DestroyClones();
            return cardSelected;

            Card SelecteEffect()
            {
                if (_cardSelected == null) return null;
                Effect effectSelected = clonesCardView[_cardSelected];
                effectSelected.Card.ClearEffects();
                effectSelected.Card.AddEffect(effectSelected);
                return effectSelected.Card;
            }

            async Task DestroyClones()
            {
                (cardView.transform.position, _cardSelected.transform.position) = (_cardSelected.transform.position, cardView.transform.position);
                List<CardView> cardViewsToDestroy = clonesCardView.Keys.Except(new List<CardView> { cardView }).ToList();
                await _moveCardHandler.MoveCardsToZone(cardViewsToDestroy, _zoneViewsManager.OutZone);
                cardViewsToDestroy.ForEach(cardView => Object.Destroy(cardView.gameObject));
                clonesCardView.Clear();
            }
        }

        public void Clicked(CardView cardView = null)
        {
            _cardSelected = cardView;
            waitForSelection.SetResult(true);
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
