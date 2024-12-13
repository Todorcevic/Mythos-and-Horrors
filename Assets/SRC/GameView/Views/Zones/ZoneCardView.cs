using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ZoneCardView : ZoneView
    {
        private Tween _hoverAnimation;
        private CardView _ownerCardView;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _hoverPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderController _invisibleHolderController;

        private CardView OwnerCardView => _ownerCardView ??= GetComponentInParent<CardView>();

        /*******************************************************************/
        public override Tween EnterZone(CardView cardView) => _invisibleHolderController.AddCardView(cardView, false);

        public override Tween ExitZone(CardView cardView) => _invisibleHolderController.RemoveCardView(cardView, false);

        public override Tween MouseEnter(CardView cardView)
        {
            _hoverAnimation?.Kill();
            (Transform invisibleHolder, Tween holdersTween) = _invisibleHolderController.SetLayout(cardView, layoutAmount: 1.5f, isInHand: false);
            _hoverPosition.localPosition = new Vector3(invisibleHolder.localPosition.x, _hoverPosition.localPosition.y, _hoverPosition.localPosition.z);
            cardView.transform.DOFullLocalMove(_hoverPosition).SetEase(Ease.OutCubic);
            return _hoverAnimation = OwnerCardView.CurrentZoneView.MouseEnter(OwnerCardView);
        }

        public override Tween MouseExit(CardView cardView)
        {
            _hoverAnimation?.Kill();
            _invisibleHolderController.ResetLayout(cardView, false);
            return _hoverAnimation = OwnerCardView.CurrentZoneView.MouseExit(OwnerCardView);
        }
    }
}