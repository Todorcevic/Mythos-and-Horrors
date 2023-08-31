using GameRules;
using System.Collections.Generic;

namespace GameView
{
    public class CardsManager
    {
        private readonly List<CardView> _allCards = new();

        /*******************************************************************/
        public void Add(CardView cardView) => _allCards.Add(cardView);

        public CardView Get(Card card) => _allCards.Find(cardView => cardView.Card == card);
    }
}
