using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardHistoriesLoaderUseCase
    {
        [Inject] private readonly FilesPath _filesPath;

        /*******************************************************************/
        public List<History> Execute()
        {
            string jsonData = File.ReadAllText(_filesPath.JSON_CARD_HISTORIES_PATH);
            return JsonConvert.DeserializeObject<List<History>>(jsonData);
        }
    }
}
