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

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            StaticContext.Container.BindInstance(false).WhenInjectedInto<InitializerComponent>();
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_AdventurerCard()
        {
            yield return LoadScenes("GamePlay");

            List<Card> cards = new()
            {
                (Card)SceneContainer.Instantiate(typeof(CardAdventurer), new object[]
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
                }),
            };

            _sut.BuildCards(cards);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();
            SpriteRenderer template = result.GetPrivateMember<SpriteRenderer>("_template");
            SpriteRenderer badge = result.GetPrivateMember<SpriteRenderer>("_badge");
            FactionAdventurerSO factionElementsExpected = result.GetPrivateMember<FactionAdventurerSO>("_cunning");

            Assert.That(result.Card, Is.EqualTo(cards.First()));
            Assert.That(result is AdventurerCardView, Is.True);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Adventurer1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo("10"));
            Assert.That(result.transform.GetTextFromThis("Agility"), Is.EqualTo("3"));
            Assert.That(template.sprite == factionElementsExpected._templateFront, Is.True, $"was of: {factionElementsExpected._templateFront}");
            Assert.That(badge.sprite == factionElementsExpected._badget, Is.True, $"was of: {factionElementsExpected._badget}");

            //yield return new WaitUntil(() => Input.anyKeyDown);
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_DeckCard()
        {
            yield return LoadScenes("GamePlay");

            List<Card> cards = new()
            {
                (Card)SceneContainer.Instantiate(typeof(CardTalent), new object[]
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
                }),
            };

            _sut.BuildCards(cards);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();
            SpriteRenderer template = result.GetPrivateMember<SpriteRenderer>("_template");
            SpriteRenderer badge = result.GetPrivateMember<SpriteRenderer>("_badge");
            SpriteRenderer healthRenderer = result.GetPrivateMember<SpriteRenderer>("_healthRenderer");
            List<SkillIconView> skillPlacer = result.GetPrivateMember<List<SkillIconView>>("_skillPlacer");
            FactionDeckSO factionElementsExpected = result.GetPrivateMember<FactionDeckSO>("_brave");

            Assert.That(result.Card, Is.EqualTo(cards.First()));
            Assert.That(result is DeckCardView, Is.True);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Aid1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Cost"), Is.EqualTo("4"));
            Assert.That(healthRenderer.gameObject.activeInHierarchy, Is.False);
            Assert.That(skillPlacer.FindAll(skillIconView => !skillIconView.IsInactive).Count, Is.EqualTo(4));
            Assert.That(template.sprite == factionElementsExpected._templateDeckFront, Is.True, $"was of: {factionElementsExpected._templateDeckFront}");
            Assert.That(badge.sprite == factionElementsExpected._badget, Is.True, $"was of: {factionElementsExpected._badget}");

            //yield return new WaitUntil(() => Input.anyKeyDown);
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_Support()
        {
            yield return LoadScenes("GamePlay");

            List<Card> cards = new()
            {
                (Card)SceneContainer.Instantiate(typeof(CardAid), new object[]
                {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Aid,
                        Code = "00001",
                        Name = "Aid1",
                        Faction = Faction.Esoteric,
                        Health= 10,
                        Sanity=6,
                        Cost=4,
                        Strength=2,
                        Wild=2
                    }
                }),
            };

            _sut.BuildCards(cards);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();
            SpriteRenderer template = result.GetPrivateMember<SpriteRenderer>("_template");
            SpriteRenderer badge = result.GetPrivateMember<SpriteRenderer>("_badge");
            SpriteRenderer healthRenderer = result.GetPrivateMember<SpriteRenderer>("_healthRenderer");
            List<SkillIconView> skillPlacer = result.GetPrivateMember<List<SkillIconView>>("_skillPlacer");
            FactionDeckSO factionElementsExpected = result.GetPrivateMember<FactionDeckSO>("_esoteric");

            Assert.That(result.Card, Is.EqualTo(cards.First()));
            Assert.That(result is DeckCardView, Is.True);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Aid1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Cost"), Is.EqualTo("4"));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo("10"));
            Assert.That(result.transform.GetTextFromThis("Sanity"), Is.EqualTo("6"));
            Assert.That(healthRenderer.gameObject.activeInHierarchy, Is.True);
            Assert.That(skillPlacer.FindAll(skillIconView => !skillIconView.IsInactive).Count, Is.EqualTo(4));
            Assert.That(template.sprite == factionElementsExpected._templateDeckFront, Is.True, $"was of: {factionElementsExpected._templateDeckFront}");
            Assert.That(badge.sprite == factionElementsExpected._badget, Is.True, $"was of: {factionElementsExpected._badget}");

            //yield return new WaitUntil(() => Input.anyKeyDown);
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_Place()
        {
            yield return LoadScenes("GamePlay");

            List<Card> cards = new()
            {
                (Card)SceneContainer.Instantiate(typeof(CardPlace), new object[]
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
                }),
            };

            _sut.BuildCards(cards);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();

            Assert.That(result.Card, Is.EqualTo(cards.First()));
            Assert.That(result is PlaceCardView, Is.True);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Place1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Enigma"), Is.EqualTo("6"));
            Assert.That(result.transform.GetTextFromThis("Hints"), Is.EqualTo("10"));

            //yield return new WaitUntil(() => Input.anyKeyDown);
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_CreatureCard()
        {
            yield return LoadScenes("GamePlay");

            List<Card> cards = new()
            {
                (Card)SceneContainer.Instantiate(typeof(CardCreature), new object[]
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
                }),
            };

            _sut.BuildCards(cards);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();
            SpriteRenderer healthRenderer = result.GetPrivateMember<SpriteRenderer>("_healthRenderer");
            List<SkillIconView> skillPlacer = result.GetPrivateMember<List<SkillIconView>>("_skillPlacer");

            Assert.That(result.Card, Is.EqualTo(cards.First()));
            Assert.That(result is SceneCardView, Is.True);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Creature1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo("6"));
            Assert.That(result.transform.GetTextFromThis("Strength"), Is.EqualTo("2"));
            Assert.That(result.transform.GetTextFromThis("Agility"), Is.EqualTo("3"));
            Assert.That(healthRenderer.gameObject.activeInHierarchy, Is.True);
            Assert.That(skillPlacer.FindAll(skillIconView => !skillIconView.IsInactive).Count, Is.EqualTo(3));

            //yield return new WaitUntil(() => Input.anyKeyDown);
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_SceneCard()
        {
            yield return LoadScenes("GamePlay");

            List<Card> cards = new()
            {
                (Card)SceneContainer.Instantiate(typeof(CardAdversity), new object[]
                {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Adversity,
                        Code = "00001",
                        Name = "Adversity1",
                    }
                }),
            };

            _sut.BuildCards(cards);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();
            SpriteRenderer healthRenderer = result.GetPrivateMember<SpriteRenderer>("_healthRenderer");
            List<SkillIconView> skillPlacer = result.GetPrivateMember<List<SkillIconView>>("_skillPlacer");

            Assert.That(result.Card, Is.EqualTo(cards.First()));
            Assert.That(result is SceneCardView, Is.True);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Adversity1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(healthRenderer.gameObject.activeInHierarchy, Is.False);
            Assert.That(skillPlacer.FindAll(skillIconView => !skillIconView.IsInactive).Count, Is.EqualTo(0));

            //yield return new WaitUntil(() => Input.anyKeyDown);
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_PlotCard()
        {
            yield return LoadScenes("GamePlay");

            List<Card> cards = new()
            {
                (Card)SceneContainer.Instantiate(typeof(CardPlot), new object[]
                {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Plot,
                        Code = "00001",
                        Name = "Plot1",
                        Eldritch= 10,
                    }
                }),
            };

            _sut.BuildCards(cards);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();

            Assert.That(result.Card, Is.EqualTo(cards.First()));
            Assert.That(result is PlotCardView, Is.True);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Plot1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Eldritch"), Is.EqualTo("10"));

            //yield return new WaitUntil(() => Input.anyKeyDown);
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_GoalCard()
        {
            yield return LoadScenes("GamePlay");

            List<Card> cards = new()
            {
                (Card)SceneContainer.Instantiate(typeof(CardGoal), new object[]
                {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Goal,
                        Code = "00001",
                        Name = "Goal1",
                        Hints= 8,
                    }
                }),
            };

            _sut.BuildCards(cards);

            CardView result = _sut.transform.GetComponentInChildren<CardView>();

            Assert.That(result.Card, Is.EqualTo(cards.First()));
            Assert.That(result is GoalCardView, Is.True);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo("Goal1"));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo("DescriptionTest1"));
            Assert.That(result.transform.GetTextFromThis("Hints"), Is.EqualTo("8"));

            //yield return new WaitUntil(() => Input.anyKeyDown);
        }
    }
}
