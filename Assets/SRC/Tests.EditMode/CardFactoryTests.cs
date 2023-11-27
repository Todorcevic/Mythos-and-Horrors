using MythsAndHorrors.PlayMode;
using NUnit.Framework;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class CardFactoryTests : OneTimeAutoInject
    {
        [Inject] private readonly CardFactory _sut;

        [Test]
        public void CardFactory_With_CardInfo()
        {
            List<CardInfo> cardInfo = new()
            {
                new()
                {
                    Code = "01603",
                    Name = "First Adventurer",
                    CardType = CardType.Adventurer,
                    Cost = 4,
                    Description = "Hola Mondo"
                },
                new()
                {
                    Code = "01501",
                    Name = "Montro2",
                    CardType = CardType.Creature,
                    Cost = 3,
                    Description = "AJJAJA"
                }
            };

            //List<Card> result = _sut.CreateCards(new List<string>() { "01501", "01603" });

            //Assert.That(result.Count, Is.EqualTo(2));
            //Assert.That(result[0].Info.Name, Is.EqualTo("First Adventurer"));
            //Assert.That(result[1].Info.CardType, Is.EqualTo(CardType.Creature));
        }
    }
}
