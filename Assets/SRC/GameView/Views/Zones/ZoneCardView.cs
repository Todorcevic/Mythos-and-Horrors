using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneCardView : ZoneView
    {
        [SerializeField, Required] private CardView _ownerCardView;
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderView _invisibleHolderView;

        /*******************************************************************/
        public override Tween MoveCard(CardView cardView) => _invisibleHolderView.AddCardView(cardView)
            .OnComplete(() => cardView.SetCurrentZoneView(this));

        public override Tween RemoveCard(CardView cardView) => _invisibleHolderView.RemoveCardView(cardView);

        public override Tween MouseDrag(CardView cardView) => DOTween.Sequence();

        public override Tween MouseEnter(CardView cardView)
        {
            //InvisibleHolder invisibleHolder = _invisibleHolderView.GetInvisibleHolder(cardView);
            //if (_invisibleHolderView.AmountOfCards > 3) invisibleHolder.SetLayoutWidth(ViewValues.INITIAL_LAYOUT_WIDTH * 1.5f);
            //_invisibleHolderView.Repositionate(cardView);
            //_hoverPosition.localPosition = new Vector3(invisibleHolder.transform.localPosition.x, _hoverPosition.localPosition.y, invisibleHolder.transform.localPosition.z);
            cardView.transform.DOLocalMoveY(0.5f, ViewValues.FAST_TIME_ANIMATION);




            return _ownerCardView.CurrentZoneView.MouseEnter(_ownerCardView);
        }

        public override Tween MouseExit(CardView cardView)
        {
            _invisibleHolderView.Repositionate(cardView);
            return _ownerCardView.CurrentZoneView.MouseExit(_ownerCardView);
        }
    }
}