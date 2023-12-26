using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using UnityEngine;
using System.Collections.Generic;

namespace MythsAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class CardGeneratorComponentTests : TestBase
    {
        [Inject] private readonly CardInfoBuilder _cardInfoBuilder;
        [Inject] private readonly CardBuilder _cardBuilder;
        [Inject] private readonly CardViewGeneratorComponent _sut;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_AdventurerCard()
        {
            Card card = _cardBuilder.BuildOfType<CardAdventurer>();

            _sut.BuildCard(card);
            CardView result = _sut.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is AdventurerCardView);
            Assert.That(result.CurrentZoneView, Is.InstanceOf<OutZoneView>());
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo(card.Info.Description));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo(card.Info.Health.ToString()));
            Assert.That(result.transform.GetTextFromThis("Agility"), Is.EqualTo(card.Info.Agility.ToString()));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_Faction()
        {
            Card card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Adventurer).WithFaction(Faction.Esoteric).GiveMe());

            _sut.BuildCard(card);
            CardView result = _sut.transform.GetComponentInChildren<CardView>(includeInactive: true);
            FactionAdventurerSO factionElementsExpected = result.GetPrivateMember<List<FactionAdventurerSO>>("_factions")
                .Find(factionAdventurerSO => factionAdventurerSO._faction == Faction.Esoteric);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.GetPrivateMember<SpriteRenderer>("_template").sprite == factionElementsExpected._templateFront,
                $"was of: {factionElementsExpected._templateFront}");
            Assert.That(result.GetPrivateMember<SpriteRenderer>("_badge").sprite == factionElementsExpected._badget,
                $"was of: {factionElementsExpected._badget}");
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_DeckCard()
        {
            Card card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Condition).WithHealth(null).GiveMe());

            _sut.BuildCard(card);
            CardView result = _sut.transform.GetComponentInChildren<CardView>(includeInactive: true);
            SpriteRenderer healthRenderer = result.GetPrivateMember<SpriteRenderer>("_healthRenderer");
            SkillIconsController skillIconsController = result.GetPrivateMember<SkillIconsController>("_skillIconsController");

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is DeckCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo(card.Info.Description));
            Assert.That(result.transform.GetTextFromThis("Cost"), Is.EqualTo(card.Info.Cost.ToString()));
            Assert.That(healthRenderer.gameObject.activeInHierarchy, Is.False);
            Assert.That(skillIconsController.GetComponentsInChildren<SkillIconView>().Length, Is.EqualTo(card.TotalChallengePoints));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_Support()
        {
            Card card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Supply)
                .WithHealth(3).WithSanity(1).WithCost(5).GiveMe());

            _sut.BuildCard(card);
            CardView result = _sut.transform.GetComponentInChildren<CardView>(includeInactive: true);
            SpriteRenderer healthRenderer = result.GetPrivateMember<SpriteRenderer>("_healthRenderer");
            SkillIconsController skillIconsController = result.GetPrivateMember<SkillIconsController>("_skillIconsController");

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is DeckCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo(card.Info.Description));
            Assert.That(result.transform.GetTextFromThis("Cost"), Is.EqualTo(card.Info.Cost.ToString()));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo(card.Info.Health.ToString()));
            Assert.That(result.transform.GetTextFromThis("Sanity"), Is.EqualTo(card.Info.Sanity.ToString()));
            Assert.That(healthRenderer.gameObject.activeSelf, Is.True);
            Assert.That(skillIconsController.GetComponentsInChildren<SkillIconView>().Length, Is.EqualTo(card.TotalChallengePoints));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_Place()
        {
            Card card = _cardBuilder.BuildOfType<CardPlace>();

            _sut.BuildCard(card);
            CardView result = _sut.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is PlaceCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo(card.Info.Description));
            Assert.That(result.transform.GetTextFromThis("Enigma"), Is.EqualTo(card.Info.Enigma.ToString()));
            Assert.That(result.transform.GetTextFromThis("Hints"), Is.EqualTo(card.Info.Hints.ToString()));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_CreatureCard()
        {
            CardCreature card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Creature)
                .WithHealth(3).WithStrength(1).WithAgility(5).WithEnemyDamage(2).WithEnemyFear(1).GiveMe()) as CardCreature;

            _sut.BuildCard(card);
            CardView result = _sut.transform.GetComponentInChildren<CardView>(includeInactive: true);
            SkillIconsController skillPlacer = result.GetPrivateMember<SkillIconsController>("_skillIconsController");

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is CreatureCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo(card.Info.Description));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo(card.Info.Health.ToString()));
            Assert.That(result.transform.GetTextFromThis("Strength"), Is.EqualTo(card.Info.Strength.ToString()));
            Assert.That(result.transform.GetTextFromThis("Agility"), Is.EqualTo(card.Info.Agility.ToString()));
            Assert.That(skillPlacer.GetComponentsInChildren<SkillIconView>().Length, Is.EqualTo(card.TotalEnemyHits));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_AdversityCard()
        {
            Card card = _cardBuilder.BuildOfType<CardAdversity>();

            _sut.BuildCard(card);
            CardView result = _sut.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is AdversityCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo(card.Info.Description));
            yield return null;
        }


        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_PlotCard()
        {
            Card card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Plot).WithEldritch(3).GiveMe());

            _sut.BuildCard(card);
            CardView result = _sut.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is PlotCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo(card.Info.Description));
            Assert.That(result.transform.GetTextFromThis("Eldritch"), Is.EqualTo(card.Info.Eldritch.ToString()));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_GoalCard()
        {
            Card card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Goal).WithHints(3).GiveMe());

            _sut.BuildCard(card);
            CardView result = _sut.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is GoalCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description"), Is.EqualTo(card.Info.Description));
            Assert.That(result.transform.GetTextFromThis("Hints"), Is.EqualTo(card.Info.Hints.ToString()));
            yield return null;
        }
    }
}
