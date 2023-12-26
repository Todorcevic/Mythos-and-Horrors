using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using Zenject;

namespace MythsAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class CardLoaderTests : SetupAutoInject
    {
        [Inject] private readonly CardLoaderUseCase _sut;
        [Inject] private readonly CardsProvider _cardsProvider;

        /*******************************************************************/
        [Test]
        public void LoadManyCards()
        {
            Card result = _sut.Execute("01501");
            _sut.Execute("01502");

            Assert.That(result, Is.TypeOf<Card01501>());
            Assert.That(result.Info.Code, Is.EqualTo("01501"));
            Assert.That(result, Is.EqualTo(_cardsProvider.GetCard("01501")));
            Assert.That(_cardsProvider.AllCards.Count, Is.EqualTo(2));
        }

        [Test]
        public void LoadCardWithHistory()
        {
            Card result = _sut.Execute("01105");

            Assert.That(result.Histories.Count, Is.EqualTo(2));
        }
    }
}
