using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneDiscardView : ZoneView
    {
        private readonly List<CardView> _allCards = new();
        private float YOffSet => _allCards.Count * ViewValues.CARD_THICKNESS;

        /*******************************************************************/
        public override Tween MoveCard(CardView cardView)
        {
            _allCards.Add(cardView);
            _movePosition.localPosition = new Vector3(0, YOffSet, 0);
            return base.MoveCard(cardView);
        }

        public override Tween RemoveCard(CardView cardView)
        {
            _allCards.Remove(cardView);
            return DOTween.Sequence();
        }

        public override void MouseDrag(CardView cardView) { }

        public override void MouseEnter(CardView cardView)
        {
            _hoverPosition.localPosition = new Vector3(0, _hoverPosition.localPosition.y + YOffSet, 0);
            base.MouseEnter(_allCards.First());
        }

        public override void MouseExit(CardView cardView)
        {
            _hoverPosition.localPosition = new Vector3(0, _hoverPosition.localPosition.y - YOffSet, 0);
            _movePosition.localPosition = new Vector3(0, YOffSet, 0);
            base.MouseExit(_allCards.First());
        }
    }
}
