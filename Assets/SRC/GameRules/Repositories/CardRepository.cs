using System.Collections.Generic;

namespace GameRules
{
    public class CardRepository
    {
        private readonly List<Card> _cards = new();

        /*******************************************************************/
        public void CreateCards()
        {
            for (int i = 0; i < 10; i++)
            {
                _cards.Add(new Card(i.ToString(), i.ToString(), CardType.Asset));
            }
        }

        /*******************************************************************/

        public Card GetCard(string id) => _cards.Find(card => card.Id == id);
        public IReadOnlyList<Card> GetAllCards() => _cards;
    }
}
