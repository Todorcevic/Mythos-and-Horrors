using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class CardsProvider
    {
        private readonly List<Card> _cards = new();

        public IReadOnlyList<Card> AllCards => _cards;

        /*******************************************************************/
        public Card GetCard(string code) => _cards.First(card => card.Info.Code == code);

        public void AddCard(Card objectCard)
        {
            _cards.Add(objectCard);
        }

        public Card[] PlayabledCards() => _cards.Where(card => card.CanPlay()).ToArray();
    }
}
