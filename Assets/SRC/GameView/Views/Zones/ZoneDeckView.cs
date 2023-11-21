using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneDeckView : ZoneView
    {
        [SerializeField, Required] protected Transform _movePosition;
        [SerializeField, Required] protected Transform _hoverPosition;
        private readonly List<CardView> _allCards = new();
        private float YOffSet => _allCards.Count * ViewValues.CARD_THICKNESS;

        /*******************************************************************/
        public override Tween MoveCard(CardView cardView)
        {
            _allCards.Add(cardView);
            _movePosition.localPosition = new Vector3(0, YOffSet, 0);
            return cardView.transform.DOFullMove(_movePosition)
               .OnComplete(() => cardView.SetCurrentZoneView(this));
        }

        public override Tween RemoveCard(CardView cardView)
        {
            _allCards.Remove(cardView);
            return DOTween.Sequence();
        }

        public override Tween MouseDrag(CardView cardView) => DOTween.Sequence();

        public override Tween MouseEnter(CardView cardView)
        {
            _hoverPosition.localPosition = new Vector3(0, _hoverPosition.localPosition.y + YOffSet, 0);
            return _allCards.First().transform.DOFullMove(_hoverPosition).SetEase(Ease.OutCubic);
        }

        public override Tween MouseExit(CardView cardView)
        {
            _hoverPosition.localPosition = new Vector3(0, _hoverPosition.localPosition.y - YOffSet, 0);
            _movePosition.localPosition = new Vector3(0, YOffSet, 0);
            return _allCards.First().transform.DOFullMove(_movePosition);
        }
    }
}
