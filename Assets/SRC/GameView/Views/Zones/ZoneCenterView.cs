using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ZoneCenterView : ZoneView
    {
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _movePosition;

        /*******************************************************************/
        public override Tween EnterZone(CardView cardView) =>
            cardView.transform.DOFullLocalMove(_movePosition, ViewValues.DEFAULT_TIME_ANIMATION)
            .Join(cardView.DisableToCenterShow());
        //.Append(cardView.transform.DOSpiral(ViewValues.DEFAULT_TIME_ANIMATION, Vector3.up, speed: 1f, frequency: 5, depth: 0, mode: SpiralMode.ExpandThenContract));

        public override Tween ExitZone(CardView cardView) => cardView.EnableFromCenterShow();

        public override Tween MouseEnter(CardView cardView) => DOTween.Sequence();

        public override Tween MouseExit(CardView cardView) => DOTween.Sequence();
    }
}
