using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ZoneRowView : ZoneView
    {
        [SerializeField] private float _layoutAmount;
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _hoverPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderController _invisibleHolderController;

        /*******************************************************************/
        public override Tween EnterZone(CardView cardView) => _invisibleHolderController.AddCardView(cardView);

        public override Tween ExitZone(CardView cardView) => _invisibleHolderController.RemoveCardView(cardView);

        public override Tween MouseEnter(CardView cardView)
        {
            (Transform invisibleHolder, Tween holdersTween) = _invisibleHolderController.SetLayout(cardView, layoutAmount: _layoutAmount);
            _hoverPosition.localPosition = new Vector3(invisibleHolder.localPosition.x, _hoverPosition.localPosition.y, _hoverPosition.localPosition.z);

            return cardView.transform.DOFullLocalMove(_hoverPosition, timeAnimation: ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.OutCubic);
        }

        public override Tween MouseExit(CardView cardView) => _invisibleHolderController.ResetLayout(cardView);

    }
}
