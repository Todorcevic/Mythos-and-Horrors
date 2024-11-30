using DG.Tweening;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ZoneHandView : ZoneRowView
    {
        private const float threshold = 1.15f;
        private Tween _hoverAnimation;

        /*******************************************************************/
        public override Tween MouseEnter(CardView cardView)
        {
            _hoverAnimation?.Kill();
            cardView.ShowBuffsAndEffects();
            float yOFFSET = cardView.GetBuffsAmount() * threshold;
            cardView.ColliderForBuffs(yOFFSET);

            return _hoverAnimation = ((Sequence)base.MouseEnter(cardView))
                .Join(cardView.transform.DOLocalMove(_hoverPosition.localPosition + (yOFFSET * Vector3.forward), ViewValues.FAST_TIME_ANIMATION));
        }

        public override Tween MouseExit(CardView cardView)
        {
            _hoverAnimation?.Kill();
            cardView.HideBuffsAndEffects();
            cardView.ColliderRestore();
            return _hoverAnimation = base.MouseExit(cardView);
        }
    }
}
