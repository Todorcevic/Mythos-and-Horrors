using MythsAndHorrors.GameRules;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class TextsLoaderUseCase
    {
        [Inject] private readonly FilesPath _filesPath;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly TextsManager _textsManager;

        /*******************************************************************/
        public void LoadGameTexts()
        {
            string jsonData = File.ReadAllText(_filesPath.JSON_GAMETEXT_PATH);
            _textsProvider.AddTexts(JsonConvert.DeserializeObject<GameText>(jsonData));
        }

        public void LoadViewTexts()
        {
            string jsonData = File.ReadAllText(_filesPath.JSON_VIEWTEXT_PATH);
            _textsManager.AddTexts(JsonConvert.DeserializeObject<ViewText>(jsonData));
        }
    }
}
