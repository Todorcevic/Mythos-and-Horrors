using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.PlayMode
{
    public class ZoneBasicView : ZoneView
    {
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _movePosition;
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _hoverPosition;

        /*******************************************************************/
        public override Tween EnterCard(CardView cardView)
        {
            cardView.SetCurrentZoneView(this);
            return cardView.transform.DOFullMove(_movePosition);
        }

        public override Tween ExitCard(CardView cardView) => DOTween.Sequence();

        public override Tween MouseEnter(CardView cardView) => cardView.transform.DOFullMove(_hoverPosition).SetEase(Ease.OutCubic);

        public override Tween MouseExit(CardView cardView) => cardView.transform.DOFullMove(_movePosition);

        public override Tween MouseDrag(CardView cardView) => DOTween.Sequence();
    }
}
