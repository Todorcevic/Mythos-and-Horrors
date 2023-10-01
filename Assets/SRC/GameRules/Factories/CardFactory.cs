using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using Zenject;

namespace Tuesday.GameRules
{
    public class CardFactory
    {
        [Inject] private readonly DiContainer _diContainer;

        /*******************************************************************/
        public List<Card> CreateCards(List<CardInfo> cardsInfo)
        {
            List<Card> allCards = new();
            foreach (CardInfo cardInfo in cardsInfo)
            {
                Type type = Assembly.GetAssembly(typeof(Card)).GetType(typeof(Card) + cardInfo.Code);
                object objectCard = _diContainer.Instantiate(type, new object[] { cardInfo });
                type.GetInterfaces().ForEach(@interface => _diContainer.Bind(@interface).FromInstance(objectCard).NonLazy());
                allCards.Add((Card)objectCard);
            }
            return allCards;
        }

    }
}
