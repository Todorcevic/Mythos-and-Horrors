using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class MultiEffectSelectionHandler : IInteractableEffectSelectionHandler
    {
        [Inject] private ZoneViewsManager _zoneViewsManager;
        [Inject] private ChaptersProvider _chaptersProvider;
        [Inject] private CardViewGeneratorComponent _cardViewGeneratorComponent;



        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly MainButtonController _buttonController;

        private TaskCompletionSource<bool> waitForSelection;
        private CardView cardSelected;

        /*******************************************************************/
        public async Task<Effect> Interact(MultiEffectSelectionGameAction multiEffectSelectionGameAction)
        {
            foreach (Effect effect in multiEffectSelectionGameAction.Effects)
            {
                await MoveToSelectorZone(effect).AsyncWaitForCompletion();
            }

            waitForSelection = new();

            if (!multiEffectSelectionGameAction.IsManadatary) _buttonController.Activate();
            _ioActivatorComponent.ActivateSensor();
            _ioActivatorComponent.ActivateUI();

            clonesCardView.Keys.ToList().ForEach(cardView => cardView.ActivateToClick());
            _avatarViewsManager.AvatarsPlayabled(clonesCardView.Keys.Select(c => c.Card).ToList()).ForEach(avatar => avatar.ActivateGlow());
            await waitForSelection.Task;

            _avatarViewsManager.AvatarsPlayabled(clonesCardView.Keys.Select(c => c.Card).ToList()).ForEach(avatar => avatar.DeactivateGlow());

            Effect effectSelected = cardSelected == null ? null : clonesCardView[cardSelected];
            clonesCardView?.Keys.ToList().ForEach(card => Object.Destroy(card.gameObject));
            clonesCardView.Clear();
            _buttonController.Deactivate();
            if (_ioActivatorComponent.IsSensorActivated) await _ioActivatorComponent.DeactivateSensor();
            if (_ioActivatorComponent.IsUIActivated) _ioActivatorComponent.DeactivateUI();

            return effectSelected;
        }

        Dictionary<CardView, Effect> clonesCardView = new();

        private Tween MoveToSelectorZone(Effect effect)
        {
            CardView cardView = _cardViewGeneratorComponent.Create(effect.Card);
            clonesCardView.Add(cardView, effect);
            ZoneView newZoneView = _zoneViewsManager.Get(_chaptersProvider.CurrentScene.SelectorZone);

            Sequence moveSequence = DOTween.Sequence()
                .Join(cardView.CurrentZoneView.ExitZone(cardView))
                .Join(cardView.Rotate())
                .Join(_zoneViewsManager.Get(_chaptersProvider.CurrentScene.SelectorZone).EnterZone(cardView));

            cardView.SetCurrentZoneView(newZoneView);
            return moveSequence;
        }

        public void Clicked(CardView cardView = null)
        {
            cardSelected = cardView;
            if (waitForSelection != null)
                waitForSelection.SetResult(true);
            waitForSelection = null;
        }
    }
}
