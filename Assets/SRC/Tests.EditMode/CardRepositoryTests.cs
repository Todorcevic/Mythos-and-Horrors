using NUnit.Framework;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class CardRepositoryTests : OneTimeAutoInject
    {
        [Inject] private readonly CardsProvider _sut;

        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            _sut.AddCard((Card)Container.Instantiate(typeof(Card01501), new object[] { new CardInfo() { Description = "DescriptionTest1", Cost = 4, CardType = CardType.Adventurer, Code = "01501", Name = "Adventurer1" } }));
            _sut.AddCard((Card)Container.Instantiate(typeof(Card01603), new object[] { new CardInfo() { Description = "DescriptionTest2", Cost = 5, CardType = CardType.Creature, Code = "01603", Name = "Monster1" } }));
        }

        [Test]
        public void CardRepository_GetCard()
        {
            Card result = _sut.GetCard("01501");

            Assert.That(result.Info.Name, Is.EqualTo("Adventurer1"));
        }

        [Test]
        public void CardRepository_GetAllCards()
        {
            IReadOnlyList<Card> result = _sut.GetAllCards();

            Assert.That(result.Count, Is.EqualTo(2));
        }
    }
}
