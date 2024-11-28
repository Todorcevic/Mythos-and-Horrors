using DG.Tweening;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ZoneHandView : ZoneRowView
    {
        private const float threshold = 1.15f;

        /*******************************************************************/
        public override Tween MouseEnter(CardView cardView)
        {
            cardView.ShowBuffsAndEffects();
            float yOFFSET = cardView.GetBuffsAmount() * threshold;
            cardView.ColliderForBuffs(yOFFSET);

            return ((Sequence)base.MouseEnter(cardView))
                .Join(cardView.transform.DOLocalMove(_hoverPosition.localPosition + (yOFFSET * Vector3.forward), ViewValues.DEFAULT_TIME_ANIMATION));
        }

        public override Tween MouseExit(CardView cardView)
        {
            cardView.HideBuffsAndEffects();
            cardView.ColliderRestore();
            return base.MouseExit(cardView);
        }
    }
}
