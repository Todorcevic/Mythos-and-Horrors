using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;
using System.Collections.Generic;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using UnityEngine;
using TMPro;

namespace MythsAndHorrors.GameView.Tests
{
    [TestFixture]
    public class CardGeneratorComponentTests : TestBase
    {
        [Inject] private readonly CardViewGeneratorComponent _sut;

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_AdventurerCard()
        {
            Card card = (Card)SceneContainer.Instantiate(typeof(CardAdventurer), new object[]
                {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Adventurer,
                        Code = "00001",
                        Name = "Adventurer1",
                        Faction = Faction.Cunning,
                        Health= 10,
                        Sanity=6,
                        Strength=2,
                        Agility=3,
                        Intelligence=4,
                        Power=5
                    }
                });

            _sut.BuildCard(card);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();
            SpriteRenderer template = result.GetPrivateMember<SpriteRenderer>("_template");
            SpriteRenderer badge = result.GetPrivateMember<SpriteRenderer>("_badge");
            FactionAdventurerSO factionElementsExpected = result.GetPrivateMember<FactionAdventurerSO>("_cunning");

            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is AdventurerCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Adventurer1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo("10"));
            Assert.That(result.transform.GetTextFromThis("Agility"), Is.EqualTo("3"));
            Assert.That(template.sprite == factionElementsExpected._templateFront, $"was of: {factionElementsExpected._templateFront}");
            Assert.That(badge.sprite == factionElementsExpected._badget, $"was of: {factionElementsExpected._badget}");

            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_DeckCard()
        {
            Card card = (Card)SceneContainer.Instantiate(typeof(CardTalent), new object[]
              {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Talent,
                        Code = "00001",
                        Name = "Aid1",
                        Faction = Faction.Brave,
                        Cost=4,
                        Strength=2,
                        Wild=2
                    }

              });

            _sut.BuildCard(card);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();
            SpriteRenderer template = result.GetPrivateMember<SpriteRenderer>("_template");
            SpriteRenderer badge = result.GetPrivateMember<SpriteRenderer>("_badge");
            SpriteRenderer healthRenderer = result.GetPrivateMember<SpriteRenderer>("_healthRenderer");
            SkillIconsController skillIconsController = result.GetPrivateMember<SkillIconsController>("_skillIconsController");
            FactionDeckSO factionElementsExpected = result.GetPrivateMember<FactionDeckSO>("_brave");

            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is DeckCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Aid1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Cost"), Is.EqualTo("4"));
            Assert.That(healthRenderer.gameObject.activeInHierarchy, Is.False);
            Assert.That(skillIconsController.GetComponentsInChildren<SkillIconView>().Length, Is.EqualTo(4));
            Assert.That(template.sprite == factionElementsExpected._templateDeckFront, $"was of: {factionElementsExpected._templateDeckFront}");
            Assert.That(badge.sprite == factionElementsExpected._badget, $"was of: {factionElementsExpected._badget}");

            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_Support()
        {
            Card card = (Card)SceneContainer.Instantiate(typeof(CardSupply), new object[]
            {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Supply,
                        Code = "00001",
                        Name = "Aid1",
                        Faction = Faction.Esoteric,
                        Health= 10,
                        Sanity=6,
                        Cost=4,
                        Strength=2,
                        Wild=2
                    }
            });

            _sut.BuildCard(card);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();
            SpriteRenderer template = result.GetPrivateMember<SpriteRenderer>("_template");
            SpriteRenderer badge = result.GetPrivateMember<SpriteRenderer>("_badge");
            SpriteRenderer healthRenderer = result.GetPrivateMember<SpriteRenderer>("_healthRenderer");
            SkillIconsController skillIconsController = result.GetPrivateMember<SkillIconsController>("_skillIconsController");
            FactionDeckSO factionElementsExpected = result.GetPrivateMember<FactionDeckSO>("_esoteric");

            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is DeckCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Aid1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Cost"), Is.EqualTo("4"));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo("10"));
            Assert.That(result.transform.GetTextFromThis("Sanity"), Is.EqualTo("6"));
            Assert.That(healthRenderer.gameObject.activeInHierarchy);
            Assert.That(skillIconsController.GetComponentsInChildren<SkillIconView>().Length, Is.EqualTo(4));
            Assert.That(template.sprite == factionElementsExpected._templateDeckFront, $"was of: {factionElementsExpected._templateDeckFront}");
            Assert.That(badge.sprite == factionElementsExpected._badget, $"was of: {factionElementsExpected._badget}");

            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_Place()
        {

            Card card = (Card)SceneContainer.Instantiate(typeof(CardPlace), new object[]
              {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Place,
                        Code = "00001",
                        Name = "Place1",
                        Hints= 10,
                        Enigma=6
                    }
              });

            _sut.BuildCard(card);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();

            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is PlaceCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Place1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Enigma"), Is.EqualTo("6"));
            Assert.That(result.transform.GetTextFromThis("Hints"), Is.EqualTo("10"));

            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_CreatureCard()
        {
            Card card = (Card)SceneContainer.Instantiate(typeof(CardCreature), new object[]
            {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Creature,
                        Code = "00001",
                        Name = "Creature1",
                        Health=6,
                        EnemyDamage= 2,
                        EnemyFear=1,
                        Strength=2,
                        Agility=3
                    }
            });

            _sut.BuildCard(card);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();
            SkillIconsController skillPlacer = result.GetPrivateMember<SkillIconsController>("_skillIconsController");

            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is CreatureCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Creature1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo("6"));
            Assert.That(result.transform.GetTextFromThis("Strength"), Is.EqualTo("2"));
            Assert.That(result.transform.GetTextFromThis("Agility"), Is.EqualTo("3"));
            Assert.That(skillPlacer.GetComponentsInChildren<SkillIconView>().Length, Is.EqualTo(3));

            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_SceneCard()
        {
            Card card = (Card)SceneContainer.Instantiate(typeof(CardAdversity), new object[]
             {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Adversity,
                        Code = "00001",
                        Name = "Adversity1",
                    }
             });

            _sut.BuildCard(card);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();

            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is AdversityCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Adversity1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));

            yield return null;
        }


        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_PlotCard()
        {

            Card card = (Card)SceneContainer.Instantiate(typeof(CardPlot), new object[]
            {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Plot,
                        Code = "00001",
                        Name = "Plot1",
                        Eldritch= 10,
                    }
            });

            _sut.BuildCard(card);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();

            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is PlotCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Plot1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Eldritch"), Is.EqualTo("10"));

            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_GoalCard()
        {
            Card card = (Card)SceneContainer.Instantiate(typeof(CardGoal), new object[]
             {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Goal,
                        Code = "00001",
                        Name = "Goal1",
                        Hints= 8,
                    }
             });

            _sut.BuildCard(card);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();

            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is GoalCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Goal1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Hints"), Is.EqualTo("8"));

            yield return null;
        }
    }
}
