using DG.Tweening;
using System.Linq;

namespace MythsAndHorrors.GameView
{
    public class ZoneBasicView : ZoneView
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
            throw new System.NotImplementedException();
        }
    }
}
