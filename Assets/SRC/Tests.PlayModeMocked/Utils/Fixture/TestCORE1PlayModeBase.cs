using MythosAndHorrors.EditMode.Tests;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class TestCORE1PlayModeBase : TestFixtureBase
    {
        [Inject] protected readonly PreparationSceneCORE1 _preparationSceneCORE1;

        protected override string SCENE_NAME => "GamePlayCORE1";
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayModeCORE1/SaveDataCORE1.json";
    }
}