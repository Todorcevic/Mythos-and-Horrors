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
        public override Tween MoveCard(CardView cardView)
        {
            cardView.SetCurrentZoneView(this);
            return _invisibleHolderView.AddCardView(cardView);
        }

        public override Tween RemoveCard(CardView cardView) => _invisibleHolderView.RemoveCardView(cardView);

        public override Tween MouseDrag(CardView cardView) => DOTween.Sequence();


        public override Tween MouseEnter(CardView cardView)
        {
            _invisibleHolderView.LocalRepositionate(cardView);
            return _ownerCardView.CurrentZoneView.MouseEnter(_ownerCardView);
        }

        public override Tween MouseExit(CardView cardView)
        {
            _invisibleHolderView.LocalRepositionate(cardView);
            return _ownerCardView.CurrentZoneView.MouseExit(_ownerCardView);
        }
    }
}