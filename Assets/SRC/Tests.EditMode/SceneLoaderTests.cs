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
        [Inject] private readonly SceneLoaderUseCase _sut;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly DataSaveLoaderUseCase _dataSaveLoaderUseCase;

        /*******************************************************************/
        [Test]
        public void LoadScene()
        {
            _dataSaveLoaderUseCase.Execute();

            _sut.Execute();

            Assert.That(_chaptersProvider.CurrentScene.Info.Name, Is.EqualTo("El encuentro"));
            Assert.That(_chaptersProvider.CurrentScene.Info.Cards.First().Info.Code, Is.EqualTo("01108"));
        }
    }
}
