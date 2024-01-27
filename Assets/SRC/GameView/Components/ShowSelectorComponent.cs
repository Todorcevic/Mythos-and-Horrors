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
        [SerializeField, Required, ChildGameObjectsOnly] private BoxCollider _blocker;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _background;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _buttonPosition;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        private List<CardView> _cardViews = new();

        public bool IsVoid => _cardViews.Count == 0;
        public bool IsMultiEffect => !_cardViews.SequenceEqual(_cardViewsManager.GetAllCanPlay());

        /*******************************************************************/
        public Tween ShowPlayables()
        {
            _cardViews = _cardViewsManager.GetAllCanPlay();
            _blocker.enabled = true;
            _cardViews.ForEach(cardView => cardView.ShowEffects());
            return Animation();
        }

        public async Task ReturnPlayables(CardView exceptThis = null)
        {
            Shutdown();
            Sequence returnSequence = DOTween.Sequence();
            _cardViews.Except(new CardView[] { exceptThis?.Card.HasMultiEffect ?? false ? exceptThis : null })
                .OrderBy(cardView => cardView.DeckPosition).ToList()
                .ForEach(cardView => returnSequence.Join(cardView.MoveToZone(_zoneViewsManager.Get(cardView.Card.CurrentZone))));
            _cardViews.Clear();
            await returnSequence.AsyncWaitForCompletion();
        }

        /*******************************************************************/
        public Tween ShowMultiEffects(Dictionary<CardView, Effect> cardViews)
        {
            _cardViews = cardViews.Keys.ToList();
            _blocker.enabled = true;
            _cardViews.ForEach(cardView => cardView.ShowEffect(cardViews[cardView]));
            return Animation();
        }

        public async Task ReturnClones()
        {
            Shutdown();
            CardView originalCardView = _cardViews[0];
            List<CardView> clones = _cardViews.Except(new[] { originalCardView }).OrderBy(cardView => cardView.DeckPosition).ToList();
            clones.ForEach(clone => clone.MoveToZone(_zoneViewsManager.CenterShowZone)
                 .OnComplete(() => Destroy(clone.gameObject)));
            await _moveCardHandler.MoveCardWithPreviewToZone(originalCardView, _zoneViewsManager.Get(originalCardView.Card.CurrentZone));
            _cardViews.Clear();
        }

        public async Task DestroyClones(CardView cardViewSelected)
        {
            Shutdown();
            CardView originalCardView = _cardViews[0];
            List<CardView> clones = _cardViews.Except(new[] { originalCardView }).OrderBy(cardView => cardView.DeckPosition).ToList();
            (originalCardView.transform.position, cardViewSelected.transform.position) = (cardViewSelected.transform.position, originalCardView.transform.position);

            Sequence sequence = DOTween.Sequence();
            clones.ForEach(clone => sequence.Join(clone.MoveToZone(_zoneViewsManager.OutZone))
                 .OnComplete(() => Destroy(clone.gameObject)));

            sequence.Join(originalCardView.MoveToZone(_zoneViewsManager.Get(originalCardView.Card.CurrentZone)));
            await sequence.AsyncWaitForCompletion();
            _cardViews.Clear();
        }

        /*******************************************************************/
        private Sequence Shutdown()
        {
            _blocker.enabled = false;
            _cardViews.ForEach(cardView => cardView.HideEffects());
            return DOTween.Sequence()
           .Join(_background.DOFade(0f, ViewValues.DEFAULT_TIME_ANIMATION))
           .Join(_mainButtonComponent.RestorePosition());
        }

        private Sequence Animation()
        {
            Sequence showCenterSequence = DOTween.Sequence()
           .Join(_background.DOFade(ViewValues.DEFAULT_FADE, ViewValues.DEFAULT_TIME_ANIMATION))
           .Join(_mainButtonComponent.MoveToThis(_buttonPosition));
            _cardViews.ForEach(cardView => showCenterSequence.Join(cardView.MoveToZone(_zoneViewsManager.SelectorZone)));
            return showCenterSequence;
        }
    }
}
