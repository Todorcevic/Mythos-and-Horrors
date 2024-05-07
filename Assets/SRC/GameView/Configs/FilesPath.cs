using Zenject;

namespace MythosAndHorrors.GameView
{
    public class FilesPath
    {
        [Inject] private readonly DataSaveUseCase _saveDataLoaderUseCase;

        private Languaje Languaje => _saveDataLoaderUseCase.DataSave.LanguajeSelected;
        public string JSON_CARDINFO_PATH => $"Assets/Data/Base/{Languaje}/CardsInfo.json";
        public string JSON_CARDEXTRAINFO_PATH => $"Assets/Data/Base/{Languaje}/CardsExtraInfo.json";
        public string JSON_SPECIALCARDINFO_PATH => $"Assets/Data/Base/{Languaje}/SpecialCardsInfo.json";
        public string JSON_CARD_HISTORIES_PATH => $"Assets/Data/Base/{Languaje}/CardsHistories.json";
        public string JSON_CHAPTERINFO_PATH => $"Assets/Data/Base/{Languaje}/ChaptersInfo.json";
        public string JSON_GAMETEXT_PATH => $"Assets/Data/Base/{Languaje}/Texts/GameText.json";
        public string JSON_VIEWTEXT_PATH => $"Assets/Data/Base/{Languaje}/Texts/ViewText.json";

        /*******************************************************************/
        public string JSON_SCENE_PATH(string sceneName) => $"Assets/Data/Base/{Languaje}/Scenes/{sceneName}/Scene.json";
        public string JSON_HISTORY_PATH(string sceneName) => $"Assets/Data/Base/{Languaje}/Scenes/{sceneName}/Histories.json";
        public string JSON_INVESTIGATOR_PATH(string investigatorCode) => $"Assets/Data/Base/{Languaje}/Investigators/{investigatorCode}.json";

        /*******************************************************************/
        public string JSON_INVESTIGATORS_PATH => $"Assets/Data/Base/{Languaje}/Investigators.json";
    }
}
