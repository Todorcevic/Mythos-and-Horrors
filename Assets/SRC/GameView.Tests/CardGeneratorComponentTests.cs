using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;
using System.Collections.Generic;
using System.Linq;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;

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
                (Card)SceneContainer.Instantiate(typeof(Card00001), new object[] { new CardInfo() { Description = "DescriptionTest1", Cost = 4, CardType = CardType.Adventurer, Code = "00001", Name = "Adventurer1" } }),
                (Card)SceneContainer.Instantiate(typeof(Card00002), new object[] { new CardInfo() { Description = "DescriptionTest2", Cost = 5, CardType = CardType.Creature, Code = "00002", Name = "Monster1" } })
            };

            _sut.BuildCards(cards);

            Assert.That(_sut.transform.GetComponentsInChildren<CardView>().Length, Is.EqualTo(2));
            Assert.That(_sut.transform.GetComponentsInChildren<CardView>().Any(cardView => cardView.Card.Info.Name == "Adventurer1"), Is.True);
            Assert.That(_sut.transform.GetComponentsInChildren<CardView>().Any(cardView => cardView.Card.Info.CardType == CardType.Creature), Is.True);
        }
    }
}
