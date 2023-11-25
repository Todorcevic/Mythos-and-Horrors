using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardFactory
    {
        [Inject] private readonly DiContainer _diContainer;
        private List<CardInfo> _allCardInfo;

        /*******************************************************************/
        public void SetCardInfo(List<CardInfo> allCardInfo)
        {
            if (_allCardInfo != null) throw new InvalidOperationException("CardInfo already loaded");
            _allCardInfo = allCardInfo ?? throw new ArgumentNullException(nameof(allCardInfo) + " allCardInfo cant be null");
        }

        public Card CreateCard(string cardCode)
        {
            if (_allCardInfo == null) throw new InvalidOperationException("CardInfo not loaded");

            CardInfo cardInfo = _allCardInfo.First(cardInfo => cardInfo.Code == cardCode);
            Type type = (Assembly.GetAssembly(typeof(Card)).GetType(typeof(Card) + cardInfo.Code)
                ?? Assembly.GetAssembly(typeof(Card)).GetType(typeof(Card) + cardInfo.CardType.ToString()))
                ?? throw new InvalidOperationException("Card not found" + cardInfo.Code + " Type: " + cardInfo.CardType.ToString());
            Card objectCard = _diContainer.Instantiate(type, new object[] { cardInfo }) as Card;
            type.GetInterfaces().ForEach(@interface => _diContainer.Bind(@interface).FromInstance(objectCard).NonLazy());
            return objectCard;
        }
    }
}
