using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class CardsProviderTests : SetupAutoInject
    {
        [Inject] private readonly CardsProvider _sut;
        [Inject] private readonly DataSaveUseCase _dataSaveLoaderUseCase;
        [Inject] private readonly TextsLoaderUseCase _textsLoaderUseCase;
        [Inject] private readonly CardLoaderUseCase _cardLoaderUseCase;
        [Inject] private readonly InvestigatorLoaderUseCase _investigatorLoaderUseCase;

        /*******************************************************************/
        [Test]
        public void GetSpecificCard()
        {
            _dataSaveLoaderUseCase.Load();
            _textsLoaderUseCase.LoadGameTexts();
            _investigatorLoaderUseCase.Execute();

            Assert.That(_sut.GetCard<Card01501>().Info.Name, Is.EqualTo("Roland Banks"));
        }
    }
}
