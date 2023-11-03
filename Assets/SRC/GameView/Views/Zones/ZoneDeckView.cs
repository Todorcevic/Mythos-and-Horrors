using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneDeckView : ZoneView, IZoneBehaviour
    {
        private readonly List<CardView> _allCards = new();
        private float YOffSet => _allCards.Count * ViewValues.CARD_THICKNESS;

        /*******************************************************************/
        public override Tween MoveCard(CardView cardView)
        {
            _allCards.Add(cardView);
            return DOTween.Sequence()
                .Join(cardView.transform.DOMove(transform.position + new Vector3(0, YOffSet, 0), ViewValues.FAST_TIME_ANIMATION))
                .Join(cardView.transform.DORotate(transform.eulerAngles, ViewValues.FAST_TIME_ANIMATION))
                .Join(cardView.transform.DOScale(transform.localScale, ViewValues.FAST_TIME_ANIMATION))
                .OnComplete(() => cardView.SetCurrentZoneView(this));
        }

        public override Tween RemoveCard(CardView cardView)
        {
            _allCards.Remove(cardView);
            return DOTween.Sequence();
        }

        void IZoneBehaviour.OnMouseDrag(CardView cardView)
        {
            throw new System.NotImplementedException();
        }

        void IZoneBehaviour.OnMouseEnter(CardView cardView)
        {
            throw new System.NotImplementedException();
        }

        void IZoneBehaviour.OnMouseExit(CardView cardView)
        {
            throw new System.NotImplementedException();
        }
    }
}
