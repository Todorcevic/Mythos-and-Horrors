using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionCommitChallengeTest : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator CommitTwoCards()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card toPlay = _cardsProvider.GetCard<Card01538>();
            Card toPlay2 = _cardsProvider.GetCard<Card01522>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);
            Task<int> totalChallengeValue = CaptureTotalChallengeValue();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay, _investigatorsProvider.Leader.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay2, _investigatorsProvider.Leader.HandZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));

            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedIn(toPlay);
            yield return ClickedIn(toPlay2);
            yield return ClickedMainButton();
            yield return ClickedMainButton();

            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.First.Hints.Value, Is.EqualTo(1));
            Assert.That(totalChallengeValue.Result, Is.EqualTo(5));
        }
    }
}
