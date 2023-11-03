using DG.Tweening;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneRowView : ZoneView, IZoneBehaviour
    {
        private const float Y_OFF_SET = 4.4f;
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderView _invisibleHolderView;

        /*******************************************************************/
        public override Tween MoveCard(CardView cardView)
        {
            return DOTween.Sequence()
                .Join(_invisibleHolderView.AddCardView(cardView))
                .Join(cardView.transform.DORotate(transform.eulerAngles, ViewValues.FAST_TIME_ANIMATION))
                .Join(cardView.transform.DOScale(transform.localScale, ViewValues.FAST_TIME_ANIMATION))
                .OnComplete(() => cardView.SetCurrentZoneView(this));
        }

        public override Tween RemoveCard(CardView cardView) => _invisibleHolderView.RemoveCardView(cardView);

        void IZoneBehaviour.OnMouseDrag(CardView cardView)
        {
            throw new System.NotImplementedException();
        }

        void IZoneBehaviour.OnMouseEnter(CardView cardView)
        {
            if (_invisibleHolderView.Repositionate(cardView) is Sequence sequence)
            {
                InvisibleHolder invisibleHolder = _invisibleHolderView.GetInvisibleHolder(cardView);
                sequence.Join(cardView.transform.DOLocalMoveY(invisibleHolder.transform.localPosition.y + Y_OFF_SET, ViewValues.FAST_TIME_ANIMATION))
                .Join(cardView.transform.DOLocalRotate(new Vector3(-45, 0, 0), ViewValues.FAST_TIME_ANIMATION));
            }
        }

        void IZoneBehaviour.OnMouseExit(CardView cardView)
        {
            if (_invisibleHolderView.Repositionate(cardView) is Sequence sequence)
            {
                sequence.Join(cardView.transform.DOLocalRotate(Vector3.zero, ViewValues.FAST_TIME_ANIMATION));
            }
        }
    }
}
