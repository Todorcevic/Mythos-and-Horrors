using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneCardView : ZoneView
    {
        private CardView _ownerCardView;
        [SerializeField, Required] private Transform _hoverPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderController _invisibleHolderController;

        private CardView OwnerCardView => _ownerCardView ??= GetComponentInParent<CardView>();

        /*******************************************************************/
        public override Tween EnterZone(CardView cardView) => _invisibleHolderController.AddCardView(cardView);

        public override Tween ExitZone(CardView cardView) => _invisibleHolderController.RemoveCardView(cardView);

        public override Tween MouseEnter(CardView cardView)
        {
            Transform invisibleHolder = _invisibleHolderController.SetLayout(cardView, layoutAmount: 1.5f);
            _hoverPosition.localPosition = new Vector3(invisibleHolder.localPosition.x, _hoverPosition.localPosition.y, _hoverPosition.localPosition.z);
            cardView.transform.DOFullLocalMove(_hoverPosition).SetEase(Ease.OutCubic);
            return OwnerCardView.CurrentZoneView.MouseEnter(OwnerCardView);
        }

        public override Tween MouseExit(CardView cardView)
        {
            _invisibleHolderController.ResetLayout(cardView);
            return OwnerCardView.CurrentZoneView.MouseExit(OwnerCardView);
        }
    }
}