using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ZoneBasicView : ZoneView
    {
        private Tween _hoverAnimation;
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _movePosition;
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _hoverPosition;

        /*******************************************************************/
        public override Tween EnterZone(CardView cardView) => cardView.transform.DOFullLocalMove(_movePosition, ViewValues.DEFAULT_TIME_ANIMATION);

        public override Tween ExitZone(CardView cardView) => DOTween.Sequence();

        public override Tween MouseEnter(CardView cardView)
        {
            _hoverAnimation?.Kill();
            _hoverAnimation = cardView.transform.DOFullLocalMove(_hoverPosition).SetEase(Ease.OutCubic); //Can be 0.1f timeAnimation
            return _hoverAnimation;
        }

        public override Tween MouseExit(CardView cardView)
        {
            _hoverAnimation?.Kill();
            _hoverAnimation = cardView.transform.DOFullLocalMove(_movePosition);
            return _hoverAnimation;
        }
    }
}
