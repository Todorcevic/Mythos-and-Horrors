using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using UnityEngine;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCreature01121bTests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator HunterBuff()
        {
            Investigator investigator = _investigatorsProvider.Second;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value1)); ;
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, SceneCORE2.North)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, SceneCORE2.East)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Third, SceneCORE2.University)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, investigator.CurrentPlace.Hints, 2)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DrawGameAction(_investigatorsProvider.First, SceneCORE2.MaskedHunter)).AsCoroutine();

            Assume.That(SceneCORE2.MaskedHunter.CurrentPlace, Is.EqualTo(investigator.CurrentPlace));

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();

            Assume.That(investigator.Hints.Value, Is.EqualTo(2));

            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();
            yield return _gameActionsProvider.Create(new DefeatCardGameAction(SceneCORE2.MaskedHunter, investigator.InvestigatorCard)).AsCoroutine();
            yield return _gameActionsProvider.Create(new ResetAllInvestigatorsTurnsGameAction()).AsCoroutine();

            gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator HunterBuffCantPay()
        {
            Investigator investigator = _investigatorsProvider.Second;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(_investigatorsProvider.Third);
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, investigator.CurrentPlace.Hints, 4)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, SceneCORE2.East.Hints, 4)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE2.MaskedHunter, investigator.DangerZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return AssertThatIsNotClickable(SceneCORE2.CurrentGoal);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(8));
        }
    }
}
