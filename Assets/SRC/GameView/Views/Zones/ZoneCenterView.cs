using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneCenterView : ZoneView
    {
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _movePosition;

        /*******************************************************************/
        public override Tween EnterZone(CardView cardView) =>
            cardView.transform.DOFullLocalMove(_movePosition, ViewValues.DEFAULT_TIME_ANIMATION)
            //.Join(transform.DOSpiral(ViewValues.DEFAULT_TIME_ANIMATION, speed: 0.25f, frequency: 50, depth: 0, mode: SpiralMode.ExpandThenContract).SetEase(Ease.Linear))
            .Join(cardView.DisableToCenterShow());

        public override Tween ExitZone(CardView cardView) => DOTween.Sequence().Join(cardView.EnableFromCenterShow());

        public override Tween MouseEnter(CardView cardView) => DOTween.Sequence();

        public override Tween MouseExit(CardView cardView) => DOTween.Sequence();
    }
}
