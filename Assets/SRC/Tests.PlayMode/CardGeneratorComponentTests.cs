using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using UnityEngine;
using System.Collections.Generic;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class CardGeneratorComponentTests : TestBase
    {
        [Inject] private readonly CardInfoBuilder _cardInfoBuilder;
        [Inject] private readonly CardBuilder _cardBuilder;
        [Inject] private readonly CardViewGeneratorComponent _cardGenerator;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_InvestigatorCard()
        {
            Card card = _cardBuilder.BuildOfType<CardInvestigator>();

            _cardGenerator.BuildCardView(card);
            CardView result = _cardGenerator.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is InvestigatorCardView);
            Assert.That(result.CurrentZoneView, Is.InstanceOf<ZoneOutView>());
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Description));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo(card.Info.Health.ToString()));
            Assert.That(result.transform.GetTextFromThis("Agility"), Is.EqualTo(card.Info.Agility.ToString()));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_Faction()
        {
            Card card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Investigator).WithFaction(Faction.Esoteric).GiveMe());

            _cardGenerator.BuildCardView(card);
            CardView result = _cardGenerator.transform.GetComponentInChildren<CardView>(includeInactive: true);
            FactionInvestigatorSO factionElementsExpected = result.GetPrivateMember<List<FactionInvestigatorSO>>("_factions")
                .Find(factionInvestigatorSO => factionInvestigatorSO._faction == Faction.Esoteric);

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
            CardTalent card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Talent).WithHealth(null).GiveMe()) as CardTalent;

            _cardGenerator.BuildCardView(card);
            CardView result = _cardGenerator.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            StatView healthStat = result.GetPrivateMember<StatView>("_health");
            SkillIconsController skillIconsController = result.GetPrivateMember<SkillIconsController>("_skillIconsController");
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is DeckCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Description));
            Assert.That(healthStat.gameObject.activeInHierarchy, Is.False);
            Assert.That(skillIconsController.GetComponentsInChildren<SkillIconView>().Length, Is.EqualTo(card.TotalChallengePoints));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_DeckCard_with_SkillIcons()
        {
            CardCondition card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Condition).WithStrength(2).WithIntelligence(4)
                .WithWild(3).WithAgility(3).GiveMe()) as CardCondition;

            _cardGenerator.BuildCardView(card);
            CardView result = _cardGenerator.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            SkillIconsController skillIconsController = result.GetPrivateMember<SkillIconsController>("_skillIconsController");
            Assert.That(skillIconsController.GetComponentsInChildren<SkillIconView>().Length, Is.EqualTo(card.TotalChallengePoints));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_DeckCard_With_Resources()
        {
            Card card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Supply).WithHealth(null).GiveMe());
            _cardGenerator.BuildCardView(card);
            DeckCardView result = _cardGenerator.transform.GetComponentInChildren<DeckCardView>(includeInactive: true);

            result.SetBulletsIcons(3);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            SkillIconsController resourceIconsController = result.GetPrivateMember<SkillIconsController>("_resourceIconsController");
            Assert.That(resourceIconsController.GetComponentsInChildren<SkillIconView>().Length, Is.EqualTo(3));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_DeckCard_With_Slot()
        {
            Card card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Supply).WithSlot(SlotType.Trinket).GiveMe());
            _cardGenerator.BuildCardView(card);
            CardView result = _cardGenerator.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            SlotController slotController = result.GetPrivateMember<SlotController>("_slotController");
            Assert.That(slotController.GetPrivateMember<SpriteRenderer>("_slot1").sprite,
                Is.EqualTo(slotController.GetPrivateMember<Sprite>("_trinket")));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_Support()
        {
            CardSupply card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Supply)
                .WithHealth(3).WithSanity(1).WithCost(5).GiveMe()) as CardSupply;

            _cardGenerator.BuildCardView(card);
            CardView result = _cardGenerator.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            StatView healthStat = result.GetPrivateMember<StatView>("_health");
            SkillIconsController skillIconsController = result.GetPrivateMember<SkillIconsController>("_skillIconsController");
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is DeckCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Description));
            Assert.That(result.transform.GetTextFromThis("Cost"), Is.EqualTo(card.Info.Cost.ToString()));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo(card.Info.Health.ToString()));
            Assert.That(result.transform.GetTextFromThis("Sanity"), Is.EqualTo(card.Info.Sanity.ToString()));
            Assert.That(healthStat.gameObject.activeSelf, Is.True);
            Assert.That(skillIconsController.GetComponentsInChildren<SkillIconView>().Length, Is.EqualTo(card.TotalChallengePoints));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_Place()
        {
            Card card = _cardBuilder.BuildOfType<CardPlace>();

            _cardGenerator.BuildCardView(card);
            CardView result = _cardGenerator.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is PlaceCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name2));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Description2));
            Assert.That(result.transform.GetTextFromThis("Enigma"), Is.EqualTo(card.Info.Enigma.ToString()));
            Assert.That(result.transform.GetTextFromThis("Hints"), Is.EqualTo(card.Info.Hints.ToString()));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_CreatureCard()
        {
            CardCreature card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Creature)
                .WithHealth(3).WithStrength(1).WithAgility(5).WithEnemyDamage(2).WithEnemyFear(1).GiveMe()) as CardCreature;

            _cardGenerator.BuildCardView(card);
            CardView result = _cardGenerator.transform.GetComponentInChildren<CardView>(includeInactive: true);
            SkillIconsController skillPlacer = result.GetPrivateMember<SkillIconsController>("_skillIconsController");

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is CreatureCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Description));
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

            _cardGenerator.BuildCardView(card);
            CardView result = _cardGenerator.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is AdversityCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Description));
            yield return null;
        }


        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_PlotCard()
        {
            Card card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Plot).WithEldritch(3).GiveMe());

            _cardGenerator.BuildCardView(card);
            CardView result = _cardGenerator.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is PlotCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Description));
            Assert.That(result.transform.GetTextFromThis("Eldritch"), Is.EqualTo(card.Info.Eldritch.ToString()));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_GoalCard()
        {
            Card card = _cardBuilder.BuildWith(_cardInfoBuilder.CreateRandom().WithCardType(CardType.Goal).WithHints(3).GiveMe());

            _cardGenerator.BuildCardView(card);
            CardView result = _cardGenerator.transform.GetComponentInChildren<CardView>(includeInactive: true);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is GoalCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Description));
            Assert.That(result.transform.GetTextFromThis("Hints"), Is.EqualTo(card.Info.Hints.ToString()));
            yield return null;
        }
    }
}
