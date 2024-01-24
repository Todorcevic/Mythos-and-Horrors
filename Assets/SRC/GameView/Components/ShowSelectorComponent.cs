using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
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
        private List<CardView> _cardViews = new();

        /*******************************************************************/
        public Tween ShowCenter(Dictionary<CardView, Effect> cardViews)
        {
            _cardViews = cardViews.Keys.ToList();
            _blocker.enabled = true;
            _cardViews.ForEach(cardView => cardView.ShowEffect(cardViews[cardView]));
            return Animation();
        }

        public Tween ShowCenter(List<CardView> cardViews)
        {
            _cardViews = cardViews;
            _blocker.enabled = true;
            cardViews.ForEach(cardView => cardView.ShowEffects());
            return Animation();
        }

        private Sequence Animation()
        {
            Sequence showCenterSequence = DOTween.Sequence()
           .Join(_background.DOFade(ViewValues.DEFAULT_FADE, ViewValues.DEFAULT_TIME_ANIMATION))
           .Join(_mainButtonComponent.MoveToThis(_buttonPosition))
           .Join(_mainButtonComponent.transform.DOScale(_buttonPosition.localScale, ViewValues.DEFAULT_TIME_ANIMATION));
            _cardViews.ForEach(cardView => showCenterSequence.Join(cardView.MoveToZone(_zoneViewsManager.SelectorZone)));
            return showCenterSequence;
        }

        public Tween Return()
        {
            Sequence returnSequence = Shutdown();
            _cardViews.ForEach(cardView => returnSequence.Join(cardView.MoveToZone(_zoneViewsManager.Get(cardView.Card.CurrentZone))));
            return returnSequence;
        }

        public Sequence Shutdown()
        {
            _blocker.enabled = false;
            _cardViews.ForEach(cardView => cardView.ClearEffects());
            return DOTween.Sequence()
           .Join(_background.DOFade(0f, ViewValues.DEFAULT_TIME_ANIMATION))
           .Join(_mainButtonComponent.RestorePosition());
        }
    }
}
