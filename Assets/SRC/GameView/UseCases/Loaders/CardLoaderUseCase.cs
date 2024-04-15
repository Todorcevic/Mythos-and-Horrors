using MythosAndHorrors.GameRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class CardLoaderUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly FilesPath _filesPath;
        [Inject] private readonly CardsProvider _cardProvider;
        [Inject] private readonly DiContainer _diContainer;
        private List<CardInfo> _allCardInfo;
        private List<CardExtraInfo> _allCardExtraInfo;

        /*******************************************************************/
        public Card Execute(string cardCode)
        {
            _allCardInfo ??= _jsonService.CreateDataFromFile<List<CardInfo>>(_filesPath.JSON_CARDINFO_PATH)
                .Concat(_jsonService.CreateDataFromFile<List<CardInfo>>(_filesPath.JSON_SPECIALCARDINFO_PATH)).ToList();

            _allCardExtraInfo ??= _jsonService.CreateDataFromFile<List<CardExtraInfo>>(_filesPath.JSON_CARDEXTRAINFO_PATH);

            CardInfo cardInfo = _allCardInfo.First(cardInfo => cardInfo.Code == cardCode);
            CardExtraInfo cardExtraInfo = _allCardExtraInfo.Find(cardExtraInfo => cardExtraInfo.Code == cardCode);

            Type type = Assembly.GetAssembly(typeof(Card)).GetType(typeof(Card) + cardInfo.Code)
                ?? throw new InvalidOperationException("Card not found" + cardInfo.Code + " Type: " + cardInfo.CardType.ToString());

            object[] parameters = cardExtraInfo != null ? new object[] { cardInfo, cardExtraInfo } : new object[] { cardInfo };
            Card newCard = _diContainer.Instantiate(type, parameters) as Card;
            _cardProvider.AddCard(newCard);
            return newCard;
        }
    }
}
