using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using UnityEngine;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class CardGeneratorComponentTests : PlayModeTestsBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator CardGeneratorComponent_Generate_InvestigatorCard()
        {
            Card card = _investigatorsProvider.First.InvestigatorCard;

            CardView result = _cardViewsManager.GetCardView(card);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(result.Card, Is.EqualTo(card));

            Assert.That(result.CurrentZoneView, Is.InstanceOf<ZoneOutView>());
            Assert.That(result.GetPrivateMember<TitleController>("_titleController").transform.GetTextFromThis("Title"), Is.EqualTo(card.Info.Name));
            Assert.That(result.GetPrivateMember<DescriptionController>("_descriptionController").transform.GetTextFromThis("Description").Contains(card.Info.Description)
                , Is.True);

            Assert.That(result.GetPrivateMember<SkillStatsController>("_skillStatsController").GetPrivateMember<StatView>("_agility")
                .transform.GetTextFromThis("Value"), Is.EqualTo(card.Info.Agility.ToString()));

            Assert.That(result.GetComponentInChildren<HealthCounterController>().GetPrivateMember<int>("AmountEnable"), Is.EqualTo(card.Info.Health));
        }
    }
}
