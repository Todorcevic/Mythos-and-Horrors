using System.Collections;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class TestBase : TestCommon
    {
        [Inject] protected readonly PreparationSceneCORE2 _preparationSceneCORE2;

        protected override string SCENE_NAME => "GamePlayCORE2";
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayModeCORE2/SaveDataCORE2.json";
    }
}