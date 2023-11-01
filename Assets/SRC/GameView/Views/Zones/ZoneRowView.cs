using DG.Tweening;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneRowView : ZoneView, IZoneBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderView _invisibleHolderView;

        /*******************************************************************/
        public override Tween MoveCard(CardView card)
        {
            return DOTween.Sequence()
                .Join(_invisibleHolderView.AddCardView(card))
                .Join(card.transform.DORotate(transform.eulerAngles, ViewValues.FAST_TIME_ANIMATION))
                .Join(card.transform.DOScale(transform.localScale, ViewValues.FAST_TIME_ANIMATION))
                .OnComplete(() => card.SetCurrentZoneView(this));
        }

        public override Tween RemoveCard(CardView card) => _invisibleHolderView.RemoveCardView(card);

        void IZoneBehaviour.OnMouseDrag(CardView cardView)
        {
            throw new System.NotImplementedException();
        }

        void IZoneBehaviour.OnMouseEnter(CardView cardView)
        {
            _invisibleHolderView.RepositionateWithThisCard(cardView);
        }

        void IZoneBehaviour.OnMouseExit(CardView cardView)
        {
            _invisibleHolderView.RepositionateExiting();
        }
    }
}
