using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    public class TestCORE2Base : SetupAutoInject
    {
        [Inject] protected readonly PreparationSceneCORE2 _preparationSceneCORE2;
        private const string JSON_SAVE_DATA_PATH = "Assets/SRC/Tests.PlayModeCORE2/SaveDataCORE2.json";

        protected SceneCORE2 SceneCORE2 => _preparationSceneCORE2.SceneCORE2;
        /*******************************************************************/
        [SetUp]
        public override void RunBeforeAnyTest()
        {
            base.RunBeforeAnyTest();
            _prepareGameRulesUseCase.Execute();
        }

        protected override void BindContainer()
        {
            base.BindContainer();
            Container.Bind<PreparationSceneCORE2>().AsSingle();
            Container.BindInstance(JSON_SAVE_DATA_PATH).WhenInjectedInto<DataSaveUseCase>();
        }
    }
}