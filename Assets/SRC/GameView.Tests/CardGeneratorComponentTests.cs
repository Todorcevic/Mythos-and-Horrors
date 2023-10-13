using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;
using System.Collections.Generic;
using System.Linq;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using UnityEngine;
using System.Reflection;
using System;

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
                        CardType = CardType.Adventurer,
                        Code = "01501",
                        Name = "Adventurer1",
                        Faction = Faction.Cunning,
                        Health= 10,
                        Sanity=6,
                        Strength=2,
                        Agility=3,
                        Intelligence=4,
                        Power=5
                    }
                }),
            };

            _sut.BuildCards(cards);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();
            SpriteRenderer temaplate = result.GetPrivateMember<SpriteRenderer>("_template");
            FactionAdventurerSO factionElementsExpected = result.GetPrivateMember<FactionAdventurerSO>("_cunning");

            Assert.That(temaplate.sprite == factionElementsExpected._templateFront, Is.True, $"was: {temaplate.sprite.name}");
            Assert.That(result is AdventurerCardView, Is.True);
            Assert.That(result.Card, Is.EqualTo(cards.First()));
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Adventurer1"));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo("10"));
            Assert.That(result.transform.GetTextFromThis("Agility"), Is.EqualTo("3"));
        }
    }
}
