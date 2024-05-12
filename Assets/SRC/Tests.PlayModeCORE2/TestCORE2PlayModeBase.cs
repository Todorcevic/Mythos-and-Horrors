using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class TestCORE2PlayModeBase : PlayModeTestsBase
    {
        [Inject] protected readonly PreparationSceneCORE2PlayModeAdapted _preparationSceneCORE2;

        protected override string SCENE_NAME => "GamePlayCORE2";
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayModeCORE2/SaveDataCORE2.json";
    }
}