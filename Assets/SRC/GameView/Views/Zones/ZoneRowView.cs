using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneRowView : ZoneView
    {
        [SerializeField, Required] protected Transform _hoverPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderView _invisibleHolderView;

        /*******************************************************************/
        public override Tween MoveCard(CardView cardView) => _invisibleHolderView.AddCardView(cardView)
              .OnComplete(() => cardView.SetCurrentZoneView(this));

        public override Tween RemoveCard(CardView cardView) => _invisibleHolderView.RemoveCardView(cardView);

        public override Tween MouseDrag(CardView cardView) => DOTween.Sequence();

        public override Tween MouseEnter(CardView cardView)
        {
            InvisibleHolder invisibleHolder = _invisibleHolderView.GetInvisibleHolder(cardView);
            if (_invisibleHolderView.AmountOfCards > 3) invisibleHolder.SetLayoutWidth(ViewValues.INITIAL_LAYOUT_WIDTH * 1.5f);
            _invisibleHolderView.Repositionate(cardView);
            _hoverPosition.localPosition = new Vector3(invisibleHolder.transform.localPosition.x, _hoverPosition.localPosition.y, invisibleHolder.transform.localPosition.z);
            return cardView.transform.DOFullMove(_hoverPosition);
        }

        public override Tween MouseExit(CardView cardView)
        {
            InvisibleHolder invisibleHolder = _invisibleHolderView.GetInvisibleHolder(cardView);
            invisibleHolder.SetLayoutWidth(ViewValues.INITIAL_LAYOUT_WIDTH);
            return _invisibleHolderView.Repositionate(cardView);
        }
    }
}
