
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardSupply01573Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DrawItem()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card01573 supply = _cardsProvider.GetCard<Card01573>();
            yield return BuildCard("01574", investigator);
            Card01574 item = _cardsProvider.GetCard<Card01574>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(item, investigator.DiscardZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(supply);
            yield return ClickedIn(item);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
            Assert.That(item.CurrentZone, Is.EqualTo(investigator.HandZone));
        }
    }
}
