using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class SceneLoaderTests : SetupAutoInject
    {
        private const string JSON_TEST_DATA_PATH = "Assets/Data/Tests/Scenes/COREScene1/Scene.json";
        [Inject] private readonly SceneLoaderUseCase _sut;
        [Inject] private readonly GameStateService _gameState;

        /*******************************************************************/
        [Test]
        public void LoadScene()
        {
            _sut.Execute(JSON_TEST_DATA_PATH);

            Assert.That(_gameState.CurrentScene.Name, Is.EqualTo("Scene1"));
            Assert.That(_gameState.CurrentScene.Cards.First().Info.Code, Is.EqualTo("01108"));
        }
    }
}
