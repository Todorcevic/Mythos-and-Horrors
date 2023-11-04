using DG.Tweening;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneRowView : ZoneView
    {
        private const float Y_OFF_SET = 4.4f;
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderView _invisibleHolderView;

        /*******************************************************************/
        public override Tween MoveCard(CardView cardView)
        {
            return _invisibleHolderView.AddCardView(cardView)
              .OnComplete(() => cardView.SetCurrentZoneView(this));
        }

        public override Tween RemoveCard(CardView cardView) => _invisibleHolderView.RemoveCardView(cardView);

        public override void MouseDrag(CardView cardView) { }

        public override void MouseEnter(CardView cardView)
        {
            InvisibleHolder invisibleHolder = _invisibleHolderView.GetInvisibleHolder(cardView);
            if (_invisibleHolderView.AllActivesInvisibleHolders.Count() > 3) invisibleHolder.SetLayoutWidth(ViewValues.INITIAL_LAYOUT_WIDTH * 1.5f);
            _invisibleHolderView.Repositionate(cardView);
            _hoverPosition.localPosition = new Vector3(invisibleHolder.transform.localPosition.x, _hoverPosition.localPosition.y, invisibleHolder.transform.localPosition.z);
            base.MouseEnter(cardView);
        }

        public override void MouseExit(CardView cardView)
        {
            InvisibleHolder invisibleHolder = _invisibleHolderView.GetInvisibleHolder(cardView);
            invisibleHolder.SetLayoutWidth(ViewValues.INITIAL_LAYOUT_WIDTH);
            _invisibleHolderView.Repositionate(cardView);
        }
    }
}
