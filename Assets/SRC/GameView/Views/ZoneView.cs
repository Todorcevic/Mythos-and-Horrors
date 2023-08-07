using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace GameView
{
    public class ZoneView : MonoBehaviour
    {
        private readonly List<CardView> _allCards = new();

        /*******************************************************************/
        public void MoveCard(CardView card)
        {
            card.transform.DOMove(transform.position, 0.5f);
            card.transform.DORotate(transform.eulerAngles, 0.5f);
            card.transform.DOScale(transform.localScale, 0.5f);

            _allCards.Add(card);
        }
    }
}
