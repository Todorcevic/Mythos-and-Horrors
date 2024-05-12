using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    public class TestCORE1Base : SetupAutoInject
    {
        [Inject] protected readonly PreparationSceneCORE1 _preparationSceneCORE1;

        private const string JSON_SAVE_DATA_PATH = "Assets/SRC/Tests.PlayModeCORE1/SaveDataCORE1.json";

        protected SceneCORE1 SceneCORE1 => _preparationSceneCORE1.SceneCORE1;

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
            Container.Bind<PreparationSceneCORE1>().AsSingle();
            Container.BindInstance(JSON_SAVE_DATA_PATH).WhenInjectedInto<DataSaveUseCase>();
        }
    }
}