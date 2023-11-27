namespace MythsAndHorrors.GameView
{
    public static class FilesPath
    {
        public const string JSON_CARDINFO_PATH = "Assets/Data/Base/CardsInfo.json";
        public const string JSON_ADVENTURERS_PATH = "Assets/Data/Base/Adventurers.json";
        public const string JSON_SAVE_DATA_PATH = "Assets/Data/Save/SaveData.json";
        public static string JSON_SCENE_PATH(string sceneName) => $"Assets/Data/Base/Scenes/{sceneName}/Scene.json";
        public static string JSON_HISTORY_PATH(string sceneName) => $"Assets/Data/Base/Scenes/{sceneName}/Histories.json";
        public static string JSON_ADVENTURER_PATH(string adventurerCode) => $"Assets/Data/Base/Adventurers/{adventurerCode}.json";
    }
}
