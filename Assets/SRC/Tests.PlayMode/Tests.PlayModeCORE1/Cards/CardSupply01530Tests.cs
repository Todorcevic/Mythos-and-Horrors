using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01530Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator PlayFromHandFreeCost()
        {
            Card01530 supply = _cardsProvider.GetCard<Card01530>();
            Investigator investigator = _investigatorsProvider.Second;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.HandZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(supply);
            Assert.That(investigator.CurrentActions.Value, Is.EqualTo(3));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.CurrentZone, Is.EqualTo(investigator.AidZone));
        }

        [UnityTest]
        public IEnumerator BuffIntelligenceToOwnerTest()
        {
            Card cardWithBuff = _cardsProvider.GetCard<Card01530>();
            Investigator investigator = _investigatorsProvider.Second;
            yield return StartingScene();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardWithBuff, investigator.AidZone).Execute().AsCoroutine();

            Assert.That(_investigatorsProvider.First.Intelligence.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Second.Intelligence.Value, Is.EqualTo(6));

            yield return _gameActionsProvider.Create<DiscardGameAction>().SetWith(cardWithBuff).Execute().AsCoroutine();

            Assert.That(_investigatorsProvider.First.Intelligence.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Second.Intelligence.Value, Is.EqualTo(5));
        }
    }
}
