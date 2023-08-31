using System.Collections.Generic;
using Zenject;

namespace GameRules
{
    public class CardRepository
    {
        private readonly List<Card> _cards = new();

        /*******************************************************************/
        public void AddCard(Card card) => _cards.Add(card);
        public Card GetCard(string code) => _cards.Find(card => card.Info.Code == code);
        public IReadOnlyList<Card> GetAllCards() => _cards;
    }
}
