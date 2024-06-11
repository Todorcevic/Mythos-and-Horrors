using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Linq;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionCommitChallengeTest : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

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

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay, investigator.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay2, investigator.HandZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));

            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedIn(toPlay);
            yield return ClickedIn(toPlay2);
            yield return ClickedMainButton();
            yield return ClickedMainButton();

            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
            Assert.That(totalChallengeValue.Result, Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator CommitDifferentInLimbo()
        {
            Investigator investigator = _investigatorsProvider.Third;
            Card toPlay = _cardsProvider.GetCard<Card01538>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuilCard("01551", investigator);
            Card01551 conditionCard = _cardsProvider.GetCard<Card01551>();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay, investigator.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(SceneCORE1.GhoulGelid, investigator.CurrentPlace)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(SceneCORE1.GhoulGelid);
            Assert.That(_gameActionsProvider.CurrentChallenge.CommitsCards.Any(), Is.False);
            yield return ClickedIn(toPlay);
            Assert.That(_gameActionsProvider.CurrentChallenge.CommitsCards.Any(), Is.True);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(toPlay.CurrentZone.ZoneType, Is.EqualTo(ZoneType.InvestigatorDiscard));
            Assert.That(conditionCard.CurrentZone.ZoneType, Is.EqualTo(ZoneType.InvestigatorDiscard));
        }
    }
}
