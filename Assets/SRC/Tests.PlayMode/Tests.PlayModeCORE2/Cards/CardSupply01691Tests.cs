
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardSupply01691Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator RetrieveDamage()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01691", investigator);
            Card01691 supply = _cardsProvider.GetCard<Card01691>();
            CardCreature creature = SceneCORE2.MaskedHunter;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedTokenButton();
            yield return ClickedIn(supply);
            yield return ClickedIn(creature);
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.Exausted.IsActive, Is.True);
            Assert.That(supply.FearRecived.Value, Is.EqualTo(1));
            Assert.That(creature.DamageRecived.Value, Is.EqualTo(2));
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(0));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }
    }
}
