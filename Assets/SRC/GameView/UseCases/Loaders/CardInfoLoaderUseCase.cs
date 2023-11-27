using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardInfoLoaderUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly CardLoaderUseCase _cardFactory;

        /*******************************************************************/
        public void Execute(string cardInfoFilePath)
        {
            List<CardInfo> allCardInfo = _jsonService.CreateDataFromFile<List<CardInfo>>(cardInfoFilePath);
            _cardFactory.SetCardInfo(allCardInfo);
        }
    }
}
