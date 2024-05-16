using MythosAndHorrors.EditMode.Tests;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class TestCORE3PlayModeBase : TestFixtureBase
    {
        [Inject] protected readonly PreparationSceneCORE3PlayModeAdapted _preparationSceneCORE3;

        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayModeCORE3/SaveDataCORE3.json";
    }
}