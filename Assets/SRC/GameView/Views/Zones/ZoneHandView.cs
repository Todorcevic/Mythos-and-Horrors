using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneHandView : ZoneView
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _hoverPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderController _invisibleHolderController;

        /*******************************************************************/
        public override Tween IntoZone(CardView cardView)
        {
            return _invisibleHolderController.AddCardView(cardView);
        }

        public override Tween OutZone(CardView cardView) => _invisibleHolderController.RemoveCardView(cardView);

        public override Tween MouseDrag(CardView cardView) => DOTween.Sequence();

        public override Tween MouseEnter(CardView cardView)
        {
            Transform invisibleHolder = _invisibleHolderController.SetLayout(cardView, layoutAmount: 2f);
            _hoverPosition.localPosition = new Vector3(invisibleHolder.localPosition.x, _hoverPosition.localPosition.y, _hoverPosition.localPosition.z);
            return cardView.transform.DOFullLocalMove(_hoverPosition).SetEase(Ease.OutCubic);
        }

        public override Tween MouseExit(CardView cardView) => _invisibleHolderController.ResetLayout(cardView);
    }
}
