namespace MythsAndHorrors.GameView
{
    public class FilesPath
    {
        public string JSON_CARDINFO_PATH = "Assets/Data/Base/CardsInfo.json";
        public string JSON_ADVENTURERS_PATH = "Assets/Data/Base/Adventurers.json";
        public string JSON_SAVE_DATA_PATH = "Assets/Data/Save/SaveData.json";
        public string JSON_SCENE_PATH(string sceneName) => $"Assets/Data/Base/Scenes/{sceneName}/Scene.json";
        public string JSON_HISTORY_PATH(string sceneName) => $"Assets/Data/Base/Scenes/{sceneName}/Histories.json";
        public string JSON_ADVENTURER_PATH(string adventurerCode) => $"Assets/Data/Base/Adventurers/{adventurerCode}.json";
    }
}
