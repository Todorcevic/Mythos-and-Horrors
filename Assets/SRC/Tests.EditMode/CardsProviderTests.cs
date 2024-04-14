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
        [Inject] private readonly DataSaveLoaderUseCase _dataSaveLoaderUseCase;
        [Inject] private readonly TextsLoaderUseCase _textsLoaderUseCase;
        [Inject] private readonly CardLoaderUseCase _cardLoaderUseCase;

        /*******************************************************************/
        [Test]
        public void GetAllCards()
        {
            _dataSaveLoaderUseCase.Execute();
            _textsLoaderUseCase.LoadGameTexts();

            _cardLoaderUseCase.Execute("01501");
            _cardLoaderUseCase.Execute("01603");

            Assert.That(_sut.GetCard<Card01501>().Info.Name, Is.EqualTo("Roland Banks"));
            Assert.That(_sut.AllCards.Count, Is.EqualTo(2));
        }
    }
}
