using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.EditMode
{
    public class CardsProvider
    {
        private readonly List<Card> _cards = new();

        /*******************************************************************/
        public Card GetCard(string code) => _cards.First(card => card.Info.Code == code);

        public IReadOnlyList<Card> GetAllCards() => _cards;

        public void AddCard(Card objectCard)
        {
            _cards.Add(objectCard);
        }
    }
}
