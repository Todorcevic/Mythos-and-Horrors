using System.IO;
using Newtonsoft.Json;
using Zenject;
using System.Collections.Generic;

namespace MythosAndHorrors.GameView
{
    public class TextsLoaderUseCase
    {
        [Inject] private readonly FilesPath _filesPath;
        [Inject] private readonly TextsManager _textsManager;

        /*******************************************************************/
        public void LoadViewTexts()
        {
            string jsonDataLocalizable = File.ReadAllText(_filesPath.JSON_LOCALIZABLE_TEXT_PATH);
            _textsManager.AddLocalizableDictionary(JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonDataLocalizable));

            string jsonDataEnums = File.ReadAllText(_filesPath.JSON_ENUMS_TEXT_PATH);
            _textsManager.AddEnumsTexts(JsonConvert.DeserializeObject<EnumsTexts>(jsonDataEnums));
        }
    }
}
