using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneCardView : ZoneView
    {
        private Tween _hoverAnimation;
        private Tween _hoverAnimation2;
        private CardView _ownerCardView;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _hoverPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderController _invisibleHolderController;

        private CardView OwnerCardView => _ownerCardView ??= GetComponentInParent<CardView>();

        /*******************************************************************/
        public override Tween EnterZone(CardView cardView) => _invisibleHolderController.AddCardView(cardView);

        public override Tween ExitZone(CardView cardView) => _invisibleHolderController.RemoveCardView(cardView);

        public override Tween MouseEnter(CardView cardView)
        {
            _hoverAnimation2?.Kill();
            (Transform invisibleHolder, Tween holdersTween) = _invisibleHolderController.SetLayout(cardView, layoutAmount: 1.5f);
            _hoverPosition.localPosition = new Vector3(invisibleHolder.localPosition.x, _hoverPosition.localPosition.y, _hoverPosition.localPosition.z);
            cardView.transform.DOFullLocalMove(_hoverPosition).SetEase(Ease.OutCubic);
            _hoverAnimation = OwnerCardView.CurrentZoneView.MouseEnter(OwnerCardView);
            return _hoverAnimation;
        }

        public override Tween MouseExit(CardView cardView)
        {
            _hoverAnimation?.Kill();
            _invisibleHolderController.ResetLayout(cardView);
            _hoverAnimation2 = OwnerCardView.CurrentZoneView.MouseExit(OwnerCardView);
            return _hoverAnimation2;
        }
    }
}