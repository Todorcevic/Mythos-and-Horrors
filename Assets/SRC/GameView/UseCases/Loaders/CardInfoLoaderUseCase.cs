using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardInfoLoaderUseCase
    {
        [Inject] private readonly FilesPath _filesPath;

        /*******************************************************************/
        public List<CardInfo> Execute()
        {
            string jsonData = File.ReadAllText(_filesPath.JSON_CARDINFO_PATH);
            return JsonConvert.DeserializeObject<List<CardInfo>>(jsonData);
        }
    }
}
