using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
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
        private const float OFFSET = 120f;
        private const float SCALE = 0.05f;
        private List<CardView> _cardViews = new();

        /*******************************************************************/
        public Tween ShowCenter(List<CardView> cardViews)
        {
            _cardViews = cardViews;
            _blocker.enabled = true;
            return Animation();

            Sequence Animation()
            {
                Sequence showCenterSequence = DOTween.Sequence().OnStart(() => _mainButtonComponent.transform.ChangeAllLayers(3))
               .Join(_background.DOFade(ViewValues.DEFAULT_FADE, ViewValues.DEFAULT_TIME_ANIMATION))
               .Join(_mainButtonComponent.transform.DOMove(ButtonPositionInUI(), ViewValues.DEFAULT_TIME_ANIMATION))
               .Join(_mainButtonComponent.transform.DOScale(_buttonPosition.localScale, ViewValues.DEFAULT_TIME_ANIMATION));
                _cardViews.ForEach(cardView => showCenterSequence.Join(cardView.MoveToZone(_zoneViewsManager.SelectorZone)));
                return showCenterSequence;
            }

            Vector3 ButtonPositionInUI()
            {
                float distanceBorderRight = (OFFSET - (Screen.width - Camera.main.WorldToScreenPoint(_buttonPosition.position).x)) * SCALE;
                return new Vector3(_buttonPosition.position.x - distanceBorderRight, _buttonPosition.position.y, _buttonPosition.position.z);
            }
        }

        public Tween Return()
        {
            Sequence returnSequence = Shutdown() as Sequence;
            _cardViews.ForEach(cardView => returnSequence.Join(cardView.MoveToZone(_zoneViewsManager.Get(cardView.Card.CurrentZone))));
            return returnSequence;
        }

        public Tween Shutdown()
        {
            _blocker.enabled = false;
            return DOTween.Sequence()
           .Join(_background.DOFade(0f, ViewValues.DEFAULT_TIME_ANIMATION))
           .Join(_mainButtonComponent.transform.DOLocalMove(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
           .Join(_mainButtonComponent.transform.DOLocalRotate(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION));
        }
    }
}
