using Zenject;

namespace MythosAndHorrors.GameView
{
    public class FilesPath
    {
        [Inject] private readonly DataSaveUseCase _saveDataLoaderUseCase;

        private Languaje Languaje => _saveDataLoaderUseCase.DataSave.LanguajeSelected;
        public string JSON_CARDINFO_PATH => $"Assets/Data/Base/{Languaje}/CardsInfo.json";
        public string JSON_CARDINFO_ALTERNATIVE_PATH => $"Assets/Data/Base/{Languaje}/CardsInfo_clean.json";
        public string JSON_SPECIALCARDINFO_PATH => $"Assets/Data/Base/{Languaje}/SpecialCardsInfo.json";
        public string JSON_CHAPTER_INFO_PATH => $"Assets/Data/Base/{Languaje}/ChaptersInfo.json";
        public string JSON_LOCALIZABLE_TEXT_PATH => $"Assets/Data/Base/{Languaje}/Texts/LocalizableText.json";
        public string JSON_ENUMS_TEXT_PATH => $"Assets/Data/Base/{Languaje}/Enums.json";

        /*******************************************************************/
        public string JSON_SCENE_PATH(string sceneName) => $"Assets/Data/Base/{Languaje}/Scenes/{sceneName}.json";
        public string JSON_INVESTIGATOR_PATH(string investigatorCode) => $"Assets/Data/Base/Investigators/{investigatorCode}.json";

        /*******************************************************************/
        public string JSON_SAVE_INVESTIGATORS_PATH => $"Assets/Data/Save/Investigators.json";
    }
}
