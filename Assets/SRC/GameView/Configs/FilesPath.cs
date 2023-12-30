namespace MythsAndHorrors.GameView
{
    public class FilesPath
    {
        public virtual string JSON_CARDINFO_PATH { get; } = "Assets/Data/Base/CardsInfo.json";
        public virtual string JSON_CARD_HISTORIES_PATH { get; } = "Assets/Data/Base/CardsHistories.json";
        public virtual string JSON_CHAPTERINFO_PATH { get; } = "Assets/Data/Base/ChaptersInfo.json";
        public virtual string JSON_INVESTIGATORS_PATH { get; } = "Assets/Data/Base/Investigators.json";
        public virtual string JSON_SAVE_DATA_PATH { get; } = "Assets/Data/Save/SaveData.json";

        public virtual string JSON_SCENE_PATH(string sceneName) => $"Assets/Data/Base/Scenes/{sceneName}/Scene.json";
        public virtual string JSON_HISTORY_PATH(string sceneName) => $"Assets/Data/Base/Scenes/{sceneName}/Histories.json";
        public virtual string JSON_INVESTIGATOR_PATH(string investigatorCode) => $"Assets/Data/Base/Investigators/{investigatorCode}.json";
    }
}
