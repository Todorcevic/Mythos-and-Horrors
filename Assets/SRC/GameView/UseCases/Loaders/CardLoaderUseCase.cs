using MythsAndHorrors.GameRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardLoaderUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly FilesPath _filesPath;
        [Inject] private readonly CardsProvider _cardProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        private List<CardInfo> _allCardInfo;
        private List<History> _allHistories;

        /*******************************************************************/
        public Card Execute(string cardCode)
        {
            _allCardInfo ??= _jsonService.CreateDataFromFile<List<CardInfo>>(_filesPath.JSON_CARDINFO_PATH)
                .Concat(_jsonService.CreateDataFromFile<List<CardInfo>>(_filesPath.JSON_CARDEXTRAINFO_PATH)).ToList();
            _allHistories ??= _jsonService.CreateDataFromFile<List<History>>(_filesPath.JSON_CARD_HISTORIES_PATH);

            CardInfo cardInfo = _allCardInfo.First(cardInfo => cardInfo.Code == cardCode);

            Type type = (Assembly.GetAssembly(typeof(Card)).GetType(typeof(Card) + cardInfo.Code)
                ?? Assembly.GetAssembly(typeof(Card)).GetType(typeof(Card) + cardInfo.CardType.ToString()))
                ?? throw new InvalidOperationException("Card not found" + cardInfo.Code + " Type: " + cardInfo.CardType.ToString());

            List<History> cardHistories = _allHistories.FindAll(history => history.Code.Contains(cardCode))
                .OrderBy(history => history.Code).ToList();

            Card newCard = _reactionablesProvider.Create(type, new object[] { cardInfo, cardHistories }) as Card;
            _cardProvider.AddCard(newCard);
            return newCard;
        }
    }
}
