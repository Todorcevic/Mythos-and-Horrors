using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.PlayMode
{
    public class ZoneRowView : ZoneView
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _hoverPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderController _invisibleHolderController;

        /*******************************************************************/
        public override Tween EnterCard(CardView cardView)
        {
            cardView.SetCurrentZoneView(this);
            return _invisibleHolderController.AddCardView(cardView);
        }

        public override Tween ExitCard(CardView cardView) => _invisibleHolderController.RemoveCardView(cardView);

        public override Tween MouseDrag(CardView cardView) => DOTween.Sequence();

        public override Tween MouseEnter(CardView cardView)
        {
            Transform invisibleHolder = _invisibleHolderController.SetLayout(cardView, layoutAmount: 1.5f);
            _hoverPosition.localPosition = new Vector3(invisibleHolder.localPosition.x, _hoverPosition.localPosition.y, invisibleHolder.localPosition.z);
            return cardView.transform.DOFullMove(_hoverPosition).SetEase(Ease.OutCubic);
        }

        public override Tween MouseExit(CardView cardView) => _invisibleHolderController.ResetLayout(cardView);

    }
}
