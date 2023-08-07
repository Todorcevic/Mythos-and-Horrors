using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameView
{
    public class CardsManager
    {
        private readonly List<CardView> _allCards = new();

        /*******************************************************************/
        public void AddCard(CardView card) => _allCards.Add(card);

        public CardView GetCard(int index) => _allCards[index];
    }
}
