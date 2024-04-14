using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShowSelectorComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SelectorBlockController _selectorBlockController;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _buttonPosition;
        [SerializeField, Required, SceneObjectsOnly] private TextMeshProUGUI _title;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly TokensPileComponent _tokensPileComponent;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        private List<CardView> _cardViews = new();

        private CardView OriginalCardView => _cardViews[0];
        private List<CardView> CardViewsOrdered => _cardViews.OrderBy(cardView => cardView.Card.CurrentZone.Cards.Count())
            .ThenBy(cardView => cardView.DeckPosition).ToList();
        public bool IsShowing => _selectorBlockController.IsActivated;
        public bool IsMultiEffect => _cardViews.Count > 1 && _cardViews.All(cardView => cardView.Card == _cardViews[0].Card);

        /******************* BASIC SHOW CENTER ***********************/
        public async Task ShowPlayables()
        {
            _cardViews = _cardViewsManager.GetAllCanPlay();
            await ShowUp();
        }

        public async Task ReturnPlayableWithActivation()
        {
            await CheckIfIsInSelectorAndReturnPlayables();
            _ioActivatorComponent.ActivateCardSensors();
        }

        public async Task CheckIfIsInSelectorAndReturnPlayables(IPlayable exceptThisPlayable = null)
        {
            if (!IsShowing) return;
            await ShowDown();
            Sequence returnSequence = DOTween.Sequence()
                .Append(_mainButtonComponent.RestorePosition())
                .Join(_tokensPileComponent.RestorePosition());

            CardView cardViewSelected = exceptThisPlayable as CardView;
            CardViewsOrdered.Except(new CardView[] { cardViewSelected })
                .ForEach(cardView => returnSequence.Join(cardView.MoveToZone(_zoneViewsManager.Get(cardView.Card.CurrentZone), Ease.InSine)));

            if (cardViewSelected != null)
                returnSequence.Append(cardViewSelected.MoveToZone(_zoneViewsManager.CenterShowZone, Ease.InSine));
            if (!exceptThisPlayable?.IsMultiEffect ?? false)
                returnSequence.Append(cardViewSelected.MoveToZone(_zoneViewsManager.Get(cardViewSelected.Card.CurrentZone), Ease.InSine));

            await returnSequence.AsyncWaitForCompletion();
            _cardViews.Clear();
        }

        /************************ MULTIEFFECTS **************************/
        public async Task ShowMultiEffects(List<CardView> cardViews)
        {
            _cardViews = cardViews;
            await ShowUp();
        }

        public async Task ReturnClones()
        {
            await ShowDown();
            Sequence returnClonesSequence = DOTween.Sequence()
                .Append(_mainButtonComponent.RestorePosition())
                .Join(_tokensPileComponent.RestorePosition());
            CardViewsOrdered.Except(new[] { OriginalCardView })
                .ForEach(clone => returnClonesSequence.Join(clone.MoveToZone(_zoneViewsManager.CenterShowZone, Ease.InSine)
                    .OnComplete(() => Destroy(clone.gameObject))));

            await returnClonesSequence
                .Join(_moveCardHandler.MoveCardViewWithPreviewToZone(OriginalCardView, _zoneViewsManager.Get(OriginalCardView.Card.CurrentZone)))
                .AsyncWaitForCompletion();
            _cardViews.Clear();
        }

        public async Task DestroyClones(CardView cardViewSelected)
        {
            await ShowDown();
            (OriginalCardView.transform.position, cardViewSelected.transform.position) = (cardViewSelected.transform.position, OriginalCardView.transform.position);
            Sequence sequence = DOTween.Sequence()
                .Append(_mainButtonComponent.RestorePosition())
                .Join(_tokensPileComponent.RestorePosition());
            CardViewsOrdered.Except(new[] { OriginalCardView })
                .ForEach(clone => sequence.Join(clone.MoveToZone(_zoneViewsManager.OutZone, Ease.InSine))
                    .OnComplete(() => Destroy(clone.gameObject)));

            await sequence
                .Join(_moveCardHandler.MoveCardViewWithPreviewToZone(OriginalCardView, _zoneViewsManager.Get(OriginalCardView.Card.CurrentZone)))
                .AsyncWaitForCompletion();
            //await sequence.AsyncWaitForCompletion();
            _cardViews.Clear();
        }

        /*******************************************************************/
        private async Task ShowDown()
        {
            await _ioActivatorComponent.DeactivateCardSensors();
            _selectorBlockController.DeactivateSelector();
            DeactiveTitle();
        }

        private async Task ShowUp()
        {
            await _ioActivatorComponent.DeactivateCardSensors();
            Sequence showCenterSequence = DOTween.Sequence()
               .Append(_mainButtonComponent.MoveToShowSelector(_buttonPosition))
               .Join(_tokensPileComponent.MoveToShowSelector(_buttonPosition))
               .Join(_selectorBlockController.ActivateSelector());
            CardViewsOrdered.ForEach(cardView => showCenterSequence.Join(cardView.MoveToZone(_zoneViewsManager.SelectorZone, Ease.InSine)));
            await showCenterSequence.AsyncWaitForCompletion();
            _ioActivatorComponent.ActivateCardSensors();
            ActivateTitle();
        }
        /*******************************************************************/

        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        private void ActivateTitle()
        {
            _title.text = _gameActionsProvider.CurrentInteractable.Description;
            _title.gameObject.SetActive(true);
        }

        private void DeactiveTitle()
        {
            _title.gameObject.SetActive(false);
            _title.text = string.Empty;
        }
    }
}
