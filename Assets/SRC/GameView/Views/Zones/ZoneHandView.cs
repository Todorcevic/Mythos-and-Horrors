using DG.Tweening;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneHandView : ZoneRowView
    {
        private const float threshold = 1.15f;

        /*******************************************************************/
        public override Tween MouseEnter(CardView cardView)
        {
            cardView.ShowBuffsAndEffects();
            int yOFFSET = cardView.GetBuffsAmount();
            cardView.ColliderForBuffs(threshold * yOFFSET);

            return ((Sequence)base.MouseEnter(cardView))
                .Join(cardView.transform.DOLocalMove(_hoverPosition.localPosition + (threshold * yOFFSET * Vector3.forward), ViewValues.FAST_TIME_ANIMATION));
        }

        public override Tween MouseExit(CardView cardView)
        {
            cardView.HideBuffsAndEffects();
            cardView.ColliderRespore();
            return base.MouseExit(cardView);
        }
    }
}
