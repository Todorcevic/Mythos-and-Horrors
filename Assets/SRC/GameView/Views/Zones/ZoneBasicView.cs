using DG.Tweening;
using System.Linq;

namespace MythsAndHorrors.GameView
{
    public class ZoneBasicView : ZoneView, IZoneBehaviour
    {
        public override Tween MoveCard(CardView card)
        {
            return DOTween.Sequence()
                .Join(card.transform.DOMove(transform.position, ViewValues.SLOW_TIME_ANIMATION))
                .Join(card.transform.DORotate(transform.eulerAngles, ViewValues.SLOW_TIME_ANIMATION))
                .Join(card.transform.DOScale(transform.localScale, ViewValues.SLOW_TIME_ANIMATION))
                .OnComplete(() => card.transform.SetParent(transform));
        }

        public override Tween RemoveCard(CardView card)
        {
            return DOTween.Sequence();
        }

        void IZoneBehaviour.OnMouseDrag(CardView cardView)
        {
            throw new System.NotImplementedException();
        }

        void IZoneBehaviour.OnMouseEnter(CardView cardView)
        {
            throw new System.NotImplementedException();
        }

        void IZoneBehaviour.OnMouseExit(CardView cardView)
        {
            throw new System.NotImplementedException();
        }
    }
}
