using System;
using System.Collections.Generic;
using System.Linq;

namespace GameRules
{
    public class CardRepository : ICardLoader
    {
        private List<Card> _cards;
        private List<Card> Cards => _cards ?? throw new InvalidOperationException("_cards is NULL");

        /*******************************************************************/
        public Card GetCard(string code) => Cards.First(card => card.Info.Code == code);

        public IReadOnlyList<Card> GetAllCards() => Cards;

        void ICardLoader.LoadCards(List<Card> cards) => _cards = cards;
    }
}
