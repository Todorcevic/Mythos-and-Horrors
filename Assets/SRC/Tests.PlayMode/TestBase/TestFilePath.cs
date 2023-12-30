using MythsAndHorrors.GameView;

namespace MythsAndHorrors.PlayMode.Tests
{
    public class TestFilePath : FilesPath
    {
        public override string JSON_CARDINFO_PATH { get; } = "Assets/Data/Tests/CardsInfo.json";
        public override string JSON_INVESTIGATORS_PATH { get; } = "Assets/Data/Tests/Investigators.json";
        public override string JSON_SAVE_DATA_PATH { get; } = "Assets/Data/Tests/Save/SaveData.json";

        public override string JSON_SCENE_PATH(string sceneName) => $"Assets/Data/Tests/Scenes/{sceneName}/Scene.json";
        public override string JSON_HISTORY_PATH(string sceneName) => $"Assets/Data/Tests/Scenes/{sceneName}/Histories.json";
        public override string JSON_INVESTIGATOR_PATH(string InvestigatorsCode) => $"Assets/Data/Tests/Investigators/{InvestigatorsCode}.json";
    }
}
