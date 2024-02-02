using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShowSelectorComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SelectorBlockController _selectorBlockController;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _buttonPosition;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        private List<CardView> _cardViews = new();

        public bool IsShowing => _selectorBlockController.IsActivated;
        public bool IsMultiEffect => _cardViews.Count > 1 && _cardViews.All(cardView => cardView.Card == _cardViews[0].Card);

        /*******************************************************************/
        public async Task ShowPlayables()
        {
            _cardViews = _cardViewsManager.GetAllCanPlay();
            _cardViews.ForEach(cardView => cardView.ShowEffects());
            await Animation();
        }

        public async Task ReturnPlayableWithActivation()
        {
            await CheckIfIsInSelectorAndReturnPlayables();
            _ioActivatorComponent.ActivateCardSensors();
        }

        public async Task CheckIfIsInSelectorAndReturnPlayables(CardView exceptThis = null)
        {
            if (!IsShowing) return;
            await Shutdown();
            Sequence returnSequence = DOTween.Sequence().Append(_mainButtonComponent.RestorePosition());
            _cardViews.Except(new CardView[] { exceptThis })
                .OrderBy(cardView => cardView.DeckPosition).ToList()
                .ForEach(cardView => returnSequence.Join(cardView.MoveToZone(_zoneViewsManager.Get(cardView.Card.CurrentZone), Ease.InSine)));
            await returnSequence.AsyncWaitForCompletion();
            _cardViews.Clear();
        }

        /*******************************************************************/
        public async Task ShowMultiEffects(Dictionary<CardView, Effect> cardViews)
        {
            _cardViews = cardViews.Keys.ToList();
            _cardViews.ForEach(cardView => cardView.ShowEffect(cardViews[cardView]));
            await Animation();
        }

        public async Task ReturnClones()
        {
            await Shutdown();
            CardView originalCardView = _cardViews[0];
            List<CardView> clones = _cardViews.Except(new[] { originalCardView }).OrderBy(cardView => cardView.DeckPosition).ToList();
            Sequence returnClonesSequence = DOTween.Sequence().Append(_mainButtonComponent.RestorePosition());
            clones.ForEach(clone => returnClonesSequence.Join(clone.MoveToZone(_zoneViewsManager.CenterShowZone, Ease.InSine)
                 .OnComplete(() => Destroy(clone.gameObject))));
            await returnClonesSequence.AsyncWaitForCompletion()
                .Join(_moveCardHandler.MoveCardWithPreviewToZone(originalCardView, _zoneViewsManager.Get(originalCardView.Card.CurrentZone)));
            _cardViews.Clear();
        }

        public async Task DestroyClones(CardView cardViewSelected)
        {
            await Shutdown();
            CardView originalCardView = _cardViews[0];
            List<CardView> clones = _cardViews.Except(new[] { originalCardView }).OrderBy(cardView => cardView.DeckPosition).ToList();
            (originalCardView.transform.position, cardViewSelected.transform.position) = (cardViewSelected.transform.position, originalCardView.transform.position);
            Sequence sequence = DOTween.Sequence().Append(_mainButtonComponent.RestorePosition());
            clones.ForEach(clone => sequence.Join(clone.MoveToZone(_zoneViewsManager.OutZone, Ease.InSine))
                 .OnComplete(() => Destroy(clone.gameObject)));
            _cardViews.Clear();
            await sequence.AsyncWaitForCompletion();
        }

        /*******************************************************************/
        private async Task Shutdown()
        {
            await _ioActivatorComponent.DeactivateCardSensors();
            _selectorBlockController.DeactivateSelector();
            _cardViews.ForEach(cardView => cardView.HideEffects());
        }

        private async Task Animation()
        {
            await _ioActivatorComponent.DeactivateCardSensors();
            Sequence showCenterSequence = DOTween.Sequence()
               .Append(_mainButtonComponent.MoveToThis(_buttonPosition).SetEase(Ease.InSine))
               .Join(_selectorBlockController.ActivateSelector());
            _cardViews.ForEach(cardView => showCenterSequence.Join(cardView.MoveToZone(_zoneViewsManager.SelectorZone, Ease.InSine)));
            await showCenterSequence.AsyncWaitForCompletion();
            _ioActivatorComponent.ActivateCardSensors();
        }
    }
}
