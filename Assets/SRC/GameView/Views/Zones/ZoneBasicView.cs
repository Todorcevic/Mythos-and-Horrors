using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneBasicView : ZoneView
    {
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _movePosition;
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _hoverPosition;

        /*******************************************************************/
        public override Tween IntoZone(CardView cardView)
        {
            return cardView.transform.DOFullMove(_movePosition);
        }

        public override Tween OutZone(CardView cardView) => DOTween.Sequence();

        public override Tween MouseEnter(CardView cardView) => cardView.transform.DOFullLocalMove(_hoverPosition).SetEase(Ease.OutCubic);

        public override Tween MouseExit(CardView cardView) => cardView.transform.DOFullLocalMove(_movePosition);

        public override Tween MouseDrag(CardView cardView) => DOTween.Sequence();
    }
}
