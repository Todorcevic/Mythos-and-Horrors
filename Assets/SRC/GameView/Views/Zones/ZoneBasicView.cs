using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneBasicView : ZoneView, IZoneBehaviour
    {
        private const float Y_OFF_SET = 4.4f;
        public override Tween MoveCard(CardView cardView)
        {
            return DOTween.Sequence()
                .Join(cardView.transform.DOMove(transform.position, ViewValues.SLOW_TIME_ANIMATION))
                .Join(cardView.transform.DORotate(transform.eulerAngles, ViewValues.SLOW_TIME_ANIMATION))
                .Join(cardView.transform.DOScale(transform.localScale, ViewValues.SLOW_TIME_ANIMATION))
                .OnComplete(() => cardView.SetCurrentZoneView(this));
        }

        public override Tween RemoveCard(CardView cardView)
        {
            return DOTween.Sequence();
        }

        void IZoneBehaviour.OnMouseDrag(CardView cardView)
        {
            throw new System.NotImplementedException();
        }

        void IZoneBehaviour.OnMouseEnter(CardView cardView)
        {
            DOTween.Sequence().Join(cardView.transform.DOLocalMoveY(transform.localPosition.y + Y_OFF_SET, ViewValues.FAST_TIME_ANIMATION))
                .Join(cardView.transform.DOLocalRotate(new Vector3(-45, 0, 0), ViewValues.FAST_TIME_ANIMATION));
        }

        void IZoneBehaviour.OnMouseExit(CardView cardView)
        {
            DOTween.Sequence().Join(cardView.transform.DOLocalMoveY(0, ViewValues.FAST_TIME_ANIMATION))
                .Join(cardView.transform.DOLocalRotate(Vector3.zero, ViewValues.FAST_TIME_ANIMATION));
        }
    }
}
