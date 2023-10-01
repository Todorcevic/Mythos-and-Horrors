using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;
using System.Collections.Generic;
using System.Linq;
using Tuesday.GameRules;
using Tuesday.GameView;

namespace Tuesday.Tests
{
    [TestFixture]
    public class CardGeneratorComponentTests : SceneTestFixture
    {
        [Inject] private readonly CardGeneratorComponent sut;

        [UnityTest]
        public IEnumerator CardGeneratorComponent_BuildCards()
        {
            yield return LoadScenes("GamePlay");

            List<Card> cards = new()
            {
                (Card)SceneContainer.Instantiate(typeof(Card00001), new object[] { new CardInfo() { Description = "Hola Mondo", Cost = 4, CardType = CardType.Adventurer, Code = "00001", Name = "First Adventurer" } }),
                (Card)SceneContainer.Instantiate(typeof(Card00002), new object[] { new CardInfo() { Description = "AJJAJA", Cost = 0, CardType = CardType.Creature, Code = "00002", Name = "Montro2" } })
            };

            sut.BuildCards(cards);

            Assert.That(sut.transform.GetComponentsInChildren<CardView>().Length, Is.EqualTo(2));
            Assert.That(sut.transform.GetComponentsInChildren<CardView>().Any(cardView => cardView.Card.Info.Name == "First Adventurer"), Is.True);
            Assert.That(sut.transform.GetComponentsInChildren<CardView>().Any(cardView => cardView.Card.Info.CardType == CardType.Creature), Is.True);

            yield return null;
        }
    }
}
