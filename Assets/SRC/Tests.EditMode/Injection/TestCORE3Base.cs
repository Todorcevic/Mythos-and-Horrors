using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    public class TestCORE3Base : SetupAutoInject
    {
        [Inject] protected readonly PreparationSceneCORE3 _preparationSceneCORE3;
        private const string JSON_SAVE_DATA_PATH = "Assets/SRC/Tests.PlayModeCORE3/SaveDataCORE3.json";

        protected SceneCORE3 SceneCORE3 => _preparationSceneCORE3.SceneCORE3;

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
            Container.Bind<PreparationSceneCORE3>().AsSingle();
            Container.BindInstance(JSON_SAVE_DATA_PATH).WhenInjectedInto<DataSaveUseCase>();
        }
    }
}