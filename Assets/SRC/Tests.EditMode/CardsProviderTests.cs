using MythosAndHorrors.GameRules;
using NUnit.Framework;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class CardsProviderTests : SetupAutoInject
    {
        [Inject] private readonly CardsProvider _sut;

        /*******************************************************************/
        [Test]
        public void GetAllCards()
        {
            _sut.AddCard((Card)Container.Instantiate(typeof(Card01501), new object[] { new CardInfo() { Description = "DescriptionTest1", Cost = 4, CardType = CardType.Investigator, Code = "01501", Name = "Investigator1" } }));
            _sut.AddCard((Card)Container.Instantiate(typeof(Card01603), new object[] { new CardInfo() { Description = "DescriptionTest2", Cost = 5, CardType = CardType.Creature, Code = "01603", Name = "Monster1" } }));

            Assert.That(_sut.GetCard("01501").Info.Name, Is.EqualTo("Investigator1"));
            Assert.That(_sut.AllCards.Count, Is.EqualTo(2));
        }
    }
}
