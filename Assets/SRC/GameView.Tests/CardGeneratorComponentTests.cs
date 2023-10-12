using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;
using System.Collections.Generic;
using System.Linq;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using UnityEngine;

namespace MythsAndHorrors.Gameview.Tests
{
    [TestFixture]
    public class CardGeneratorComponentTests : SceneTestFixture
    {
        [Inject] private readonly CardGeneratorComponent _sut;

        [UnityTest]
        public IEnumerator CardGeneratorComponent_BuildCards()
        {
            yield return LoadScenes("GamePlay");

            List<Card> cards = new()
            {
                (Card)SceneContainer.Instantiate(typeof(Card01501), new object[]
                {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        Cost = 4,
                        CardType = CardType.Adventurer,
                        Code = "00001",
                        Name = "Adventurer1",
                        Faction=Faction.Cunning
                    }
                }),
                //(Card)SceneContainer.Instantiate(typeof(Card01603), new object[] { new CardInfo() { Description = "DescriptionTest2", Cost = 5, CardType = CardType.Creature, Code = "00002", Name = "Monster1" } })
            };

            _sut.BuildCards(cards);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();


            //Assert.That(_sut.transform.GetComponentsInChildren<CardView>().Length, Is.EqualTo(1));
            //Assert.That(_sut.transform.GetComponentsInChildren<CardView>().Any(cardView => cardView.Card.Info.Name == "Adventurer1"), Is.True);
            //Assert.That(_sut.transform.GetComponentsInChildren<CardView>().Any(cardView => cardView.Card.Info.CardType == CardType.Adventurer), Is.True);
            Assert.That(result.Card.Info.Name, Is.EqualTo("Adventurer1"));
            Assert.That(result.Card.Info.CardType, Is.EqualTo(CardType.Adventurer));
            Assert.That(result.Card.Info.Description, Is.EqualTo("DescriptionTest1"));
            Assert.That(result.Card.Info.Cost, Is.EqualTo(4));
            Assert.That(result.Card.Info.Code, Is.EqualTo("00001"));
            Assert.That(result.Card.Info.Faction, Is.EqualTo(Faction.Cunning));

            yield return new WaitForSeconds(25);
        }
    }
}
