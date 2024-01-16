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
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        [Test]
        public void LoadScene()
        {
            _sut.Execute(JSON_TEST_DATA_PATH);

            Assert.That(_chaptersProvider.CurrentScene.Info.Name, Is.EqualTo("Scene1"));
            Assert.That(_chaptersProvider.CurrentScene.Info.Cards.First().Info.Code, Is.EqualTo("01108"));
            //Assert.That(_chaptersProvider.CurrentScene.Info.Resource.Info.Code, Is.EqualTo("Resource"));
        }
    }
}
