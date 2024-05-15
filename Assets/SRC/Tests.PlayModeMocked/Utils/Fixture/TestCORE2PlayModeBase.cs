using MythosAndHorrors.EditMode.Tests;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class TestCORE2PlayModeBase : TestFixtureBase
    {
        [Inject] protected readonly PreparationSceneCORE2 _preparationSceneCORE2;

        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayModeCORE2/SaveDataCORE2.json";
    }
}