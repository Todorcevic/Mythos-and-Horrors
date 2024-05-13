using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class TestCORE1PlayModeBase : TestFixtureBase
    {
        [Inject] protected readonly PreparationSceneCORE1PlayModeAdapted _preparationSceneCORE1;

        protected override string SCENE_NAME => "GamePlayCORE1";
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayModeCORE1/SaveDataCORE1.json";
    }

    public class TestCORE2PlayModeBase : TestFixtureBase
    {
        [Inject] protected readonly PreparationSceneCORE1PlayModeAdapted _preparationSceneCORE2;

        protected override string SCENE_NAME => "GamePlayCORE2";
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayModeCORE2/SaveDataCORE2.json";
    }

    public class TestCORE3PlayModeBase : TestFixtureBase
    {
        [Inject] protected readonly PreparationSceneCORE1PlayModeAdapted _preparationSceneCORE3;

        protected override string SCENE_NAME => "GamePlayCORE3";
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayModeCORE3/SaveDataCORE3.json";
    }
}