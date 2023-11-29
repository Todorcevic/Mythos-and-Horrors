using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class OutZoneView : ZoneView
    {
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _movePosition;
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _hoverPosition;

        /*******************************************************************/
        public override Tween EnterCard(CardView cardView)
        {
            cardView.SetCurrentZoneView(this);
            return cardView.transform.DOFullMove(_movePosition).OnComplete(() => cardView.Off());
        }

        public override Tween ExitCard(CardView cardView) => DOTween.Sequence().OnStart(() => cardView.On());

        public override Tween MouseEnter(CardView cardView) => DOTween.Sequence();

        public override Tween MouseExit(CardView cardView) => DOTween.Sequence();

        public override Tween MouseDrag(CardView cardView) => DOTween.Sequence();
    }
}
