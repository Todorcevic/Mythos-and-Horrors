using System.Collections.Generic;
using Zenject;

namespace GameRules
{
    public class CardRepository
    {
        [Inject] private readonly DiContainer _container;
        private readonly List<Card> _cards = new();

        /*******************************************************************/
        public void CreateCards()
        {
            for (int i = 0; i < 10; i++)
            {
                Card newCard = new() { Id = i.ToString(), Name = i.ToString(), Type = CardType.Asset };
                _container.Inject(newCard);
                _cards.Add(newCard);
            }

            Card00001 otherCard = new() { Id = "00001", Name = "00001", Type = CardType.Encounter };
            _container.Inject(otherCard);
            _container.Bind<IEndReactionable>().FromInstance(otherCard).NonLazy();
            _cards.Add(otherCard);
        }

        /*******************************************************************/

        public Card GetCard(string id) => _cards.Find(card => card.Id == id);
        public IReadOnlyList<Card> GetAllCards() => _cards;
    }
}
