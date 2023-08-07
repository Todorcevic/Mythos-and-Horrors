using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameView
{
    public class CardsManager
    {
        private readonly List<CardView> _allCards;

        /*******************************************************************/
        public void AddCard(CardView card) => _allCards.Add(card);
    }
}
