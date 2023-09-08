using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace GameRules
{
    public class CardRepository : ICardLoader
    {
        private const string JSON_DATA_PATH = "Assets/Data/Tuesday.json";

        [Inject] private readonly ISerializer _serializer;
        [Inject] private readonly DiContainer _diContainer;

        private readonly List<Card> _cards = new();

        /*******************************************************************/
        public Card GetCard(string code) => _cards.First(card => card.Info.Code == code);
        public IReadOnlyList<Card> GetAllCards() => _cards ?? throw new InvalidOperationException("_cards is NULL");

        void ICardLoader.LoadCards()
        {
            List<CardInfo> allCardInfo = _serializer.CreateDataFromFile<List<CardInfo>>(JSON_DATA_PATH);

            foreach (CardInfo cardInfo in allCardInfo)
            {
                Type type = Type.GetType(typeof(Card) + cardInfo.Code);
                object objectCard = _diContainer.Instantiate(type, new object[] { cardInfo });
                type.GetInterfaces().ForEach(@interface => _diContainer.Bind(@interface).FromInstance(objectCard).NonLazy());
                _cards.Add((Card)objectCard);
            }
        }
    }
}
