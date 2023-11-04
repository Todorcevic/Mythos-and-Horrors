using DG.Tweening;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneBasicView : ZoneView
    {
        [SerializeField, Required] protected Transform _movePosition;
        [SerializeField, Required] protected Transform _hoverPosition;
        [SerializeField, Required] protected Transform _showPosition;

        /*******************************************************************/
        public override Tween MoveCard(CardView cardView)
        {
            return cardView.transform.DOFullMove(_movePosition)
                .OnComplete(() => cardView.SetCurrentZoneView(this));
        }

        public override Tween RemoveCard(CardView cardView)
        {
            return DOTween.Sequence();
        }

        public override void MouseEnter(CardView cardView)
        {
            cardView.transform.DOFullMove(_hoverPosition);
        }

        public override void MouseExit(CardView cardView)
        {
            cardView.transform.DOFullMove(_movePosition);
        }

        public override void MouseDrag(CardView cardView) { }
    }
}
