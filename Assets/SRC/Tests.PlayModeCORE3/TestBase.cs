using System.Collections;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class TestBase : PlayModeTestsBase
    {
        [Inject] protected readonly PreparationScene3PlayModeAdapted _preparationSceneCORE3;

        protected override string SCENE_NAME => "GamePlayCORE3";
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayModeCORE3/SaveDataCORE3.json";
    }
}