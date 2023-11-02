using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneHandView : ZoneView, IZoneBehaviour
    {
        private const float Z_OFF_SET = 8.5f;
        private const float Y_OFF_SET = 6f;
        public float scaleCard;
        public float z;
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderView _invisibleHolderView;

        /*******************************************************************/
        public override Tween MoveCard(CardView cardView)
        {
            return DOTween.Sequence()
            .Join(_invisibleHolderView.AddCardView(cardView))
            .Join(cardView.transform.DORotate(transform.eulerAngles, ViewValues.FAST_TIME_ANIMATION))
            .Join(cardView.transform.DOScale(transform.localScale, ViewValues.FAST_TIME_ANIMATION))
            .OnComplete(() => cardView.SetCurrentZoneView(this));
        }

        public override Tween RemoveCard(CardView cardView) => _invisibleHolderView.RemoveCardView(cardView);

        void IZoneBehaviour.OnMouseDrag(CardView cardView)
        {
            throw new NotImplementedException();
        }

        void IZoneBehaviour.OnMouseEnter(CardView cardView)
        {
            if (_invisibleHolderView.Repositionate(cardView, layout: 48) is Sequence sequence)
            {
                sequence.Join(cardView.transform.DOLocalMoveZ(_invisibleHolderView.GetInvisibleHolder(cardView).transform.localPosition.z + Z_OFF_SET, ViewValues.FAST_TIME_ANIMATION))
                //.Join(cardView.transform.DOScale(scaleCard, ViewValues.FAST_TIME_ANIMATION));
                .Join(cardView.transform.DOLocalMoveY(_invisibleHolderView.GetInvisibleHolder(cardView).transform.localPosition.y + Y_OFF_SET, ViewValues.FAST_TIME_ANIMATION));
            }
        }

        void IZoneBehaviour.OnMouseExit(CardView cardView)
        {
            if (_invisibleHolderView.Repositionate(cardView) is Sequence sequence)
            {
                sequence.Join(cardView.transform.DOScale(ViewValues.CARD_ORIGINAL_SCALE, ViewValues.FAST_TIME_ANIMATION));
            }
        }
    }
}
