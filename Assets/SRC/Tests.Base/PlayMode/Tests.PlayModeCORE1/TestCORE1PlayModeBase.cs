using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class TestCORE1PlayModeBase : PlayModeTestsBase
    {
        [Inject] protected readonly PreparationSceneCORE1PlayModeAdapted _preparationSceneCORE1;

        protected override string SCENE_NAME => "GamePlayCORE1";
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.Base/PlayMode/Tests.PlayModeCORE1/SaveDataCORE1.json";
    }
}
