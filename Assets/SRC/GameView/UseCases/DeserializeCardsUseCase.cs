using GameRules;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using Zenject;

namespace GameView
{
    public class DeserializeCardsUseCase
    {
        [Inject] private readonly ISerializer _serializer;
        [Inject] private readonly DiContainer _diContainer;

        /*******************************************************************/
        public List<Card> Load(string filePath)
        {
            List<CardInfo> allCardInfo = _serializer.CreateDataFromFile<List<CardInfo>>(filePath);
            List<Card> allCards = new();

            foreach (CardInfo cardInfo in allCardInfo)
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
