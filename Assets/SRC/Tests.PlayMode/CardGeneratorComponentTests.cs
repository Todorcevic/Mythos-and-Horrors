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
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_InvestigatorCard()
        {
            Card card = _investigatorsProvider.First.InvestigatorCard;

            CardView result = _cardViewsManager.GetCardView(card);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is InvestigatorCardView);
            Assert.That(result.CurrentZoneView, Is.InstanceOf<ZoneOutView>());
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Description));
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo(card.Info.Health.ToString()));
            Assert.That(result.transform.GetTextFromThis("Agility"), Is.EqualTo(card.Info.Agility.ToString()));
            Assert.That(result.GetPrivateMember<SpriteRenderer>("_badge").sprite, Is.Not.Null);
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_Faction()
        {
            Card card = _investigatorsProvider.First.InvestigatorCard;

            CardView result = _cardViewsManager.GetCardView(card);
            FactionInvestigatorSO factionElementsExpected = result.GetPrivateMember<List<FactionInvestigatorSO>>("_factions")
                .Find(factionInvestigatorSO => factionInvestigatorSO._faction == Faction.Brave);

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
            CardTalent card = _cardsProvider.GetCard<CardTalent>("01525");

            CardView result = _cardViewsManager.GetCardView(card);
            SkillIconView skillIcon = result.GetPrivateMember<SkillIconsController>("_skillIconsController").GetComponentInChildren<SkillIconView>();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(result.Card, Is.EqualTo(card));
            Assert.That(result is DeckCardView);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Description));
            Assert.That(result.GetPrivateMember<StatView>("_health").gameObject.activeInHierarchy, Is.False);
            Assert.That(result.GetPrivateMember<SkillIconsController>("_skillIconsController").GetComponentsInChildren<SkillIconView>().Length,
                Is.EqualTo(card.TotalChallengePoints));
            Assert.That(skillIcon.GetPrivateMember<SpriteRenderer>("_skillIcon").sprite, Is.EqualTo(result.GetPrivateMember<Sprite>("_skillStrengthIcon")));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_DeckCard_With_Resources()
        {
            CardSupply card = _cardsProvider.GetCard<CardSupply>("01516");

            DeckCardView result = (DeckCardView)_cardViewsManager.GetCardView(card);
            result.SetBulletsIcons(3);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            SkillIconsController resourceIconsController = result.GetPrivateMember<SkillIconsController>("_resourceIconsController");
            SkillIconView skillIcon = resourceIconsController.GetComponentInChildren<SkillIconView>();
            Assert.That(resourceIconsController.GetComponentsInChildren<SkillIconView>().Length, Is.EqualTo(3));
            Assert.That(skillIcon.GetPrivateMember<SpriteRenderer>("_skillIcon").sprite, Is.EqualTo(result.GetPrivateMember<Sprite>("_resourceBulletIcon")));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_DeckCard_With_Slot()
        {
            CardSupply card = _cardsProvider.GetCard<CardSupply>("01516");

            DeckCardView result = (DeckCardView)_cardViewsManager.GetCardView(card);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            SlotController slotController = result.GetPrivateMember<SlotController>("_slotController");
            Assert.That(slotController.GetPrivateMember<SpriteRenderer>("_slot1").sprite,
                Is.EqualTo(slotController.GetPrivateMember<Sprite>("_item")));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_Support()
        {
            CardSupply card = _cardsProvider.GetCard<CardSupply>("01518");

            DeckCardView result = (DeckCardView)_cardViewsManager.GetCardView(card);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            StatView healthStat = result.GetPrivateMember<StatView>("_health");
            SkillIconsController skillIconsController = result.GetPrivateMember<SkillIconsController>("_skillIconsController");
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
            Card card = _cardsProvider.GetCard("01112");

            PlaceCardView result = (PlaceCardView)_cardViewsManager.GetCardView(card);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.transform.GetTextFromThis("Enigma"), Is.EqualTo(card.Info.Enigma.ToString()));
            Assert.That(result.transform.GetTextFromThis("Hints"), Is.EqualTo(card.Info.Hints.ToString()));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_CreatureCard()
        {
            CardCreature card = _cardsProvider.GetCard<CardCreature>("01118");

            CreatureCardView result = (CreatureCardView)_cardViewsManager.GetCardView(card);
            SkillIconsController skillPlacer = result.GetPrivateMember<SkillIconsController>("_skillIconsController");

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.transform.GetTextFromThis("Health"), Is.EqualTo(card.Info.Health.ToString()));
            Assert.That(result.transform.GetTextFromThis("Strength"), Is.EqualTo(card.Info.Strength.ToString()));
            Assert.That(result.transform.GetTextFromThis("Agility"), Is.EqualTo(card.Info.Agility.ToString()));
            Assert.That(skillPlacer.GetComponentsInChildren<SkillIconView>().Length, Is.EqualTo(card.TotalEnemyHits));

            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_AdversityCard()
        {
            CardAdversity card = _cardsProvider.GetCard<CardAdversity>("01167");

            AdversityCardView result = (AdversityCardView)_cardViewsManager.GetCardView(card);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Description));
            yield return null;
        }


        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_PlotCard()
        {
            Card card = _cardsProvider.GetCard("01105");

            PlotCardView result = (PlotCardView)_cardViewsManager.GetCardView(card);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Flavor));
            Assert.That(result.transform.GetTextFromThis("Eldritch"), Is.EqualTo(card.Info.Eldritch.ToString()));
            yield return null;
        }

        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_GoalCard()
        {
            Card card = _cardsProvider.GetCard("01108");

            GoalCardView result = (GoalCardView)_cardViewsManager.GetCardView(card);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.transform.GetTextFromThis("Description").Contains(card.Info.Flavor));
            Assert.That(result.transform.GetTextFromThis("Hints"), Is.EqualTo(card.Info.Hints.ToString()));
            yield return null;
        }
    }
}
