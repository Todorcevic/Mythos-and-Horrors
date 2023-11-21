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
            _cardShowerComponent.ShowCard(cardView);
            _ownerCardView.CardSensor.OnMouseEnter();
            return DOTween.Sequence();
        }

        public override Tween MouseExit(CardView cardView)
        {
            _cardShowerComponent.HideCard();
            _ownerCardView.CardSensor.OnMouseExit();
            return DOTween.Sequence();
        }
    }
}