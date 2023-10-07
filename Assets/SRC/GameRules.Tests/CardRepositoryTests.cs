using NUnit.Framework;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameRules.Tests
{
    [TestFixture]
    public class CardRepositoryTests : OneTimeAutoInject
    {
        [Inject] private readonly CardRepository _sut;

        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            List<Card> cards = new()
            {
                (Card)Container.Instantiate(typeof(Card01501), new object[] { new CardInfo() { Description = "DescriptionTest1", Cost = 4, CardType = CardType.Adventurer, Code = "00001", Name = "Adventurer1" } }),
                (Card)Container.Instantiate(typeof(Card01603), new object[] { new CardInfo() { Description = "DescriptionTest2", Cost = 5, CardType = CardType.Creature, Code = "00002", Name = "Monster1" } })
            };

            ((ICardLoader)_sut).LoadCards(cards);
        }

        [Test]
        public void CardRepository_GetCard()
        {
            Card result = _sut.GetCard("00001");

            Assert.That(result.Info.Name, Is.EqualTo("First Adventurer"));
        }

        [Test]
        public void CardRepository_GetAllCards()
        {
            IReadOnlyList<Card> result = _sut.GetAllCards();

            Assert.That(result.Count, Is.EqualTo(2));
        }
    }
}
