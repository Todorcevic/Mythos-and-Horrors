
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardSupply01582Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator RetrieveDamage()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            yield return BuildCard("01582", investigator);
            Card01582 supply = _cardsProvider.GetCard<Card01582>();
            CardCreature creature = SceneCORE2.MaskedHunter;
            CardCreature creature2 = SceneCORE2.Peter;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature2, investigator2.DangerZone).Start().AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedTokenButton();
            yield return ClickedIn(supply);
            yield return ClickedIn(creature2);
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.Exausted.IsActive, Is.True);
            Assert.That(supply.FearRecived.Value, Is.EqualTo(1));
            Assert.That(creature2.DamageRecived.Value, Is.EqualTo(2));
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(0));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }
    }
}
