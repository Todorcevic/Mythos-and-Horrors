using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCreature01121bTests : TestCORE2Preparation
    {
        [UnityTest]
        public IEnumerator HunterBuff()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value1)); ;
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, SceneCORE2.North)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Second, SceneCORE2.West)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Third, SceneCORE2.University)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Second, _investigatorsProvider.Second.CurrentPlace.Hints, 2)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DrawGameAction(_investigatorsProvider.First, SceneCORE2.MaskedHunter)).AsCoroutine();

            Assume.That(SceneCORE2.MaskedHunter.CurrentPlace, Is.EqualTo(_investigatorsProvider.Second.CurrentPlace));

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Second));
            yield return ClickedIn(_investigatorsProvider.Second.CurrentPlace);
            yield return ClickedIn(_investigatorsProvider.Second.InvestigatorCard);
            yield return ClickedMainButton();

            Assume.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(2));

            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();
            yield return _gameActionsProvider.Create(new DefeatCardGameAction(SceneCORE2.MaskedHunter, _investigatorsProvider.Second.InvestigatorCard)).AsCoroutine();
            yield return _gameActionsProvider.Create(new ResetAllInvestigatorsTurnsGameAction()).AsCoroutine();

            gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Second));
            yield return ClickedIn(_investigatorsProvider.Second.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(3));
        }
    }
}
