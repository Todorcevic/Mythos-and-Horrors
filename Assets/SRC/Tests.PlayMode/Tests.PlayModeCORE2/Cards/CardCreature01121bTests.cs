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
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.First, SceneCORE2.North).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE2.East).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Third, SceneCORE2.University).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(investigator, investigator.CurrentPlace.Hints, 2).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(_investigatorsProvider.First, SceneCORE2.MaskedHunter).Execute().AsCoroutine();

            Assume.That(SceneCORE2.MaskedHunter.CurrentPlace, Is.EqualTo(investigator.CurrentPlace));

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();

            Assume.That(investigator.Hints.Value, Is.EqualTo(2));

            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();
            yield return _gameActionsProvider.Create<DefeatCardGameAction>().SetWith(SceneCORE2.MaskedHunter, investigator.InvestigatorCard).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<ResetAllInvestigatorsTurnsGameAction>().Execute().AsCoroutine();

            gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
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
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(investigator, investigator.CurrentPlace.Hints, 4).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(investigator, SceneCORE2.East.Hints, 4).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.MaskedHunter, investigator.DangerZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return AssertThatIsNotClickable(SceneCORE2.CurrentGoal);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(8));
        }
    }
}
