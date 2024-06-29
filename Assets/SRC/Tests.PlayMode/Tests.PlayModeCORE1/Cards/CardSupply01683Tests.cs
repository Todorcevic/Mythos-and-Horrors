
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01683Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator HealthAlly()
        {
            yield return StartingScene(withResources: true);
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01683", investigator);
            Card01683 supplyCard = _cardsProvider.GetCard<Card01683>();
            Card01533 ally = _cardsProvider.GetCard<Card01533>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supplyCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ally, _investigatorsProvider.Second.AidZone)).AsCoroutine();

            Dictionary<Stat, int> stats = new()
            {
                { _investigatorsProvider.First.DamageRecived, 4},
                { _investigatorsProvider.First.FearRecived, 2},
                { _investigatorsProvider.Second.DamageRecived, 2},
                { _investigatorsProvider.Second.FearRecived, 2},
                { _investigatorsProvider.Third.DamageRecived, 1},
                { _investigatorsProvider.Fourth.FearRecived, 1},
                { ally.FearRecived, 1}
            };
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(stats)).AsCoroutine();
            Assert.That(supplyCard.AmountSupplies.Value, Is.EqualTo(4));

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(supplyCard);
            yield return ClickedIn(_investigatorsProvider.Second.InvestigatorCard);
            yield return ClickedIn(supplyCard);
            yield return ClickedIn(ally);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Second.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(ally.FearRecived.Value, Is.EqualTo(0));
            Assert.That(supplyCard.AmountSupplies.Value, Is.EqualTo(2));
        }
    }
}
