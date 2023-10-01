using System;
using System.Collections.Generic;
using System.Linq;

namespace Tuesday.GameRules
{
    public class CardRepository : ICardLoader
    {
        private List<Card> _cards;

        /*******************************************************************/
        public Card GetCard(string code) => _cards.First(card => card.Info.Code == code);

        public IReadOnlyList<Card> GetAllCards() => _cards;

        void ICardLoader.LoadCards(List<Card> cards)
        {
            if (_cards != null) throw new InvalidOperationException("Cards already loaded");
            _cards = cards ?? throw new ArgumentNullException(nameof(cards) + " cards cant be null");
        }
    }
}
