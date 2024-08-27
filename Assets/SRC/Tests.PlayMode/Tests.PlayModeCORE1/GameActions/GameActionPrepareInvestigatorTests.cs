using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Linq;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionPrepareInvestigatorTests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator PermanentAtStart()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01694", investigator);
            Card01694 supply = _cardsProvider.GetCard<Card01694>();

            yield return PlaceOnlyScene();
            Task taskGameAction = _gameActionsProvider.Create<PrepareInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.IsInPlay.IsTrue, Is.True);
        }

        [UnityTest]
        public IEnumerator Mulligan()
        {
            if (TestsType != TestsType.Unit) yield break;
            Investigator investigator = _investigatorsProvider.First;

            yield return PlaceOnlyScene();
            Task taskGameAction = _gameActionsProvider.Create<PrepareInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return ClickedIn(investigator.DiscardZone.Cards.Last());
            yield return ClickedIn(investigator.DiscardZone.Cards.Last());
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.HandSize, Is.EqualTo(5));
        }
    }
}
