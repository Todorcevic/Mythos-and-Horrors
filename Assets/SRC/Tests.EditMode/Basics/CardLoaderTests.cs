using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class CardLoaderTests : SetupAutoInject
    {
        [Inject] private readonly CardLoaderUseCase _sut;
        [Inject] private readonly DataSaveUseCase _dataSaveLoaderUseCase;
        [Inject] private readonly TextsLoaderUseCase _textsLoaderUseCase;

        /*******************************************************************/
        [SetUp]
        public void SetUp()
        {
            _dataSaveLoaderUseCase.Load();
            _textsLoaderUseCase.LoadGameTexts();
        }

        [Test]
        public void LoadManyCards()
        {
            Card result = _sut.Execute("01501");
            _sut.Execute("01502");

            Assert.That(result, Is.TypeOf<Card01501>());
            Assert.That(result.Info.Code, Is.EqualTo("01501"));
            Assert.That(result, Is.EqualTo(_cardsProvider.GetCard<Card01501>()));
            Assert.That(_cardsProvider.AllCards.Count(), Is.EqualTo(2));
        }

        [Test]
        public void LoadCardWithHistory()
        {
            CardPlot result = _sut.Execute("01105") as CardPlot;

            Assert.That(result.RevealHistory.Description, Is.Not.Empty);
        }
    }
}
