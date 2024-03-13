using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShowSelectorComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SelectorBlockController _selectorBlockController;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _buttonPosition;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly TokensPileComponent _tokensPileComponent;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        private List<CardView> _cardViews = new();

        private CardView OriginalCardView => _cardViews[0];
        private List<CardView> CardViewsOrdered => _cardViews.OrderBy(cardView => cardView.Card.CurrentZone.Cards.Count)
            .ThenBy(cardView => cardView.DeckPosition).ToList();
        public bool IsShowing => _selectorBlockController.IsActivated;
        public bool IsMultiEffect => _cardViews.Count > 1 && _cardViews.All(cardView => cardView.Card == _cardViews[0].Card);

        /******************* BASIC SHOW CENTER ***********************/
        public async Task ShowPlayables()
        {
            _cardViews = _cardViewsManager.GetAllCanPlay();
            await Animation();
        }

        public async Task ReturnPlayableWithActivation()
        {
            await CheckIfIsInSelectorAndReturnPlayables();
            _ioActivatorComponent.ActivateCardSensors();
        }

        public async Task CheckIfIsInSelectorAndReturnPlayables(IPlayable exceptThisPlayable = null)
        {
            if (!IsShowing) return;
            await Shutdown();
            Sequence returnSequence = DOTween.Sequence()
                .Append(_mainButtonComponent.RestorePosition())
                .Join(_tokensPileComponent.RestorePosition());
            CardViewsOrdered.Except(new CardView[] { exceptThisPlayable as CardView })
                .ForEach(cardView => returnSequence.Join(cardView.MoveToZone(_zoneViewsManager.Get(cardView.Card.CurrentZone), Ease.InSine)));
            await returnSequence.AsyncWaitForCompletion();
            _cardViews.Clear();
        }

        /************************ MULTIEFFECTS **************************/
        public async Task ShowMultiEffects(List<CardView> cardViews)
        {
            _cardViews = cardViews;
            await Animation();
        }

        public async Task ReturnClones()
        {
            await Shutdown();
            Sequence returnClonesSequence = DOTween.Sequence()
                .Append(_mainButtonComponent.RestorePosition())
                .Join(_tokensPileComponent.RestorePosition());
            CardViewsOrdered.Except(new[] { OriginalCardView })
                .ForEach(clone => returnClonesSequence.Join(clone.MoveToZone(_zoneViewsManager.CenterShowZone, Ease.InSine)
                    .OnComplete(() => Destroy(clone.gameObject))));
            await returnClonesSequence.AsyncWaitForCompletion()
                .Join(_moveCardHandler.MoveCardWithPreviewToZone(OriginalCardView, _zoneViewsManager.Get(OriginalCardView.Card.CurrentZone)));
            _cardViews.Clear();
        }

        public async Task DestroyClones(CardView cardViewSelected)
        {
            await Shutdown();
            (OriginalCardView.transform.position, cardViewSelected.transform.position) = (cardViewSelected.transform.position, OriginalCardView.transform.position);
            Sequence sequence = DOTween.Sequence()
                .Append(_mainButtonComponent.RestorePosition())
                .Join(_tokensPileComponent.RestorePosition());
            CardViewsOrdered.Except(new[] { OriginalCardView })
                .ForEach(clone => sequence.Join(clone.MoveToZone(_zoneViewsManager.OutZone, Ease.InSine))
                    .OnComplete(() => Destroy(clone.gameObject)));
            _cardViews.Clear();
            await sequence.AsyncWaitForCompletion();
        }

        /*******************************************************************/
        private async Task Shutdown()
        {
            await _ioActivatorComponent.DeactivateCardSensors();
            _selectorBlockController.DeactivateSelector();
        }

        private async Task Animation()
        {
            await _ioActivatorComponent.DeactivateCardSensors();
            Sequence showCenterSequence = DOTween.Sequence().SetId("ShowCenter")
               .Append(_mainButtonComponent.MoveToShowSelector(_buttonPosition))
               .Join(_tokensPileComponent.MoveToShowSelector(_buttonPosition))
               .Join(_selectorBlockController.ActivateSelector());
            CardViewsOrdered.ForEach(cardView => showCenterSequence.Join(cardView.MoveToZone(_zoneViewsManager.SelectorZone, Ease.InSine)));
            await showCenterSequence.AsyncWaitForCompletion();
            _ioActivatorComponent.ActivateCardSensors();
        }
    }
}
