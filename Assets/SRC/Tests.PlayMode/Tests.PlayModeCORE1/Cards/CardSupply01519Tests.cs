using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01519Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator ActivateToHealInvestigator()
        {
            yield return StartingScene(withResources: true);

            Card01519 supplyCard = _cardsProvider.GetCard<Card01519>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supplyCard, _investigatorsProvider.First.AidZone)).AsCoroutine();
            Dictionary<Stat, int> stats = new()
            {
                { _investigatorsProvider.First.Health, 4},
                { _investigatorsProvider.First.Sanity, 2},
                { _investigatorsProvider.Second.Health, 2},
                { _investigatorsProvider.Second.Sanity, 2},
                { _investigatorsProvider.Third.Sanity, 1},
                { _investigatorsProvider.Fourth.Health, 1}
            };
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(stats)).AsCoroutine();
            Assert.That(supplyCard.AmountSupplies.Value, Is.EqualTo(3));

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            yield return ClickedIn(supplyCard);
            yield return ClickedClone(_investigatorsProvider.Second.AvatarCard, 0);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Second.Health.Value, Is.EqualTo(3));
            Assert.That(supplyCard.AmountSupplies.Value, Is.EqualTo(2));
        }
    }
}
