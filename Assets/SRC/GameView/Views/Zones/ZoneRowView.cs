using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneRowView : ZoneView
    {
        private const float threshold = 1.15f;
        [SerializeField] private bool _showEffects;
        [SerializeField] private float _layoutAmount;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _hoverPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderController _invisibleHolderController;

        /*******************************************************************/
        public override Tween EnterZone(CardView cardView) => _invisibleHolderController.AddCardView(cardView);

        public override Tween ExitZone(CardView cardView) => _invisibleHolderController.RemoveCardView(cardView);

        public override Tween MouseEnter(CardView cardView)
        {
            int yOFFSET = 0;
            if (_showEffects)
            {
                cardView.ShowBuffsAndEffects();
                yOFFSET = cardView.GetBuffsAmount();
                cardView.ColliderForBuffs(threshold * yOFFSET);
            }

            (Transform invisibleHolder, Tween holdersTween) = _invisibleHolderController.SetLayout(cardView, layoutAmount: _layoutAmount);
            _hoverPosition.localPosition = new Vector3(invisibleHolder.localPosition.x, _hoverPosition.localPosition.y, _hoverPosition.localPosition.z);
            return cardView.transform.DOFullLocalMove(_hoverPosition)
                .Join(cardView.transform.DOLocalMove(_hoverPosition.localPosition + (threshold * yOFFSET * Vector3.forward), ViewValues.FAST_TIME_ANIMATION))
                .SetEase(Ease.OutCubic);
        }

        public override Tween MouseExit(CardView cardView)
        {
            if (_showEffects)
            {
                cardView.HideBuffsAndEffects();
                cardView.ColliderRespore();
            }

            return _invisibleHolderController.ResetLayout(cardView);
        }
    }
}
