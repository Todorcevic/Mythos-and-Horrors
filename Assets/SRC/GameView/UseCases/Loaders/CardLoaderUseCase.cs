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
        [Inject] private readonly DiContainer _diContainer;
        private List<CardInfo> _allCardInfo;
        private List<CardInfo> _allCardInfo_Alternative;

        /*******************************************************************/
        public Card Execute(string cardCode)
        {
            _allCardInfo ??= _jsonService.CreateDataFromFile<List<CardInfo>>(_filesPath.JSON_CARDINFO_PATH)
                .Concat(_jsonService.CreateDataFromFile<List<CardInfo>>(_filesPath.JSON_SPECIALCARDINFO_PATH)).ToList();

            _allCardInfo_Alternative ??= _jsonService.CreateDataFromFile<List<CardInfo>>(_filesPath.JSON_CARDINFO_ALTERNATIVE_PATH); //TODO: Remove this line when allcardsinfo is clean

            CardInfo cardInfo = _allCardInfo_Alternative.FirstOrDefault(cardInfo => cardInfo.Code == cardCode) ?? _allCardInfo.First(cardInfo => cardInfo.Code == cardCode);
            Type type = Assembly.GetAssembly(typeof(Card)).GetType(typeof(Card) + cardInfo.Code)
                ?? throw new InvalidOperationException("Card not found" + cardInfo.Code + " Type: " + cardInfo.CardType.ToString());

            Card newCard = _diContainer.Instantiate(type, new object[] { cardInfo }) as Card;
            return newCard;
        }
    }
}
