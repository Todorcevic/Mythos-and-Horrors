using Tuesday.GameRules;
using System.Collections.Generic;
using Zenject;

namespace Tuesday.GameView
{
    public class DeserializeCardsInfoUseCase
    {
        [Inject] private readonly JsonService _jsonService;

        /*******************************************************************/
        public List<CardInfo> CreateFrom(string filePath) => _jsonService.CreateDataFromFile<List<CardInfo>>(filePath);
    }
}
