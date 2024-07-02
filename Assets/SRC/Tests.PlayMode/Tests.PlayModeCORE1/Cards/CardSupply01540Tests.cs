
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01540Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator PlayFromHandFreeCost()
        {
            Investigator investigator = _investigatorsProvider.Second;
            yield return BuildCard("01540", investigator);
            Card01540 supply = _cardsProvider.GetCard<Card01540>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(supply);
            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(3));
            yield return ClickedIn(supply);
            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(3));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.CurrentZone, Is.EqualTo(investigator.HandZone));
        }
    }
}
