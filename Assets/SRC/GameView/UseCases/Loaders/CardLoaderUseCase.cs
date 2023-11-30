using MythsAndHorrors.GameRules;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardLoaderUseCase
    {
        private readonly DiContainer _diContainer;
        private readonly CardsProvider _cardProvider;
        private readonly List<CardInfo> _allCardInfo;
        private readonly ReactionablesProvider _reactionablesProvider;

        /*******************************************************************/
        public CardLoaderUseCase(DiContainer diContainer, CardsProvider cardProvider, ReactionablesProvider reactionablesProvider, CardInfoLoaderUseCase cardInfoLoader)
        {
            _diContainer = diContainer;
            _cardProvider = cardProvider;
            _reactionablesProvider = reactionablesProvider;
            _allCardInfo = cardInfoLoader.Execute();
        }

        /*******************************************************************/
        public Card Execute(string cardCode)
        {
            if (_allCardInfo == null) throw new InvalidOperationException("CardInfo not loaded");

            CardInfo cardInfo = _allCardInfo.First(cardInfo => cardInfo.Code == cardCode);
            Type type = (Assembly.GetAssembly(typeof(Card)).GetType(typeof(Card) + cardInfo.Code)
                ?? Assembly.GetAssembly(typeof(Card)).GetType(typeof(Card) + cardInfo.CardType.ToString()))
                ?? throw new InvalidOperationException("Card not found" + cardInfo.Code + " Type: " + cardInfo.CardType.ToString());
            Card newCard = _diContainer.Instantiate(type, new object[] { cardInfo }) as Card;
            type.GetInterfaces().OfType<IStartReactionable>().ForEach(startReactionable => _reactionablesProvider.AddReactionable(startReactionable));
            type.GetInterfaces().OfType<IEndReactionable>().ForEach(endReactionable => _reactionablesProvider.AddReactionable(endReactionable));
            _cardProvider.AddCard(newCard);
            return newCard;
        }
    }
}
