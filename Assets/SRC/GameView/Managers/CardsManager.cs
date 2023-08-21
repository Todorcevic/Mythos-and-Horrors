using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameView
{
    public class CardsManager
    {
        private readonly List<CardView> _allCards = new();

        /*******************************************************************/
        public void Add(CardView card) => _allCards.Add(card);

        public CardView Get(int index) => _allCards[index];

        public CardView Get(string id) => _allCards.Find(card => card.Id == id);
    }
}
