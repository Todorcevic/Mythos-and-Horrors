using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class GameActionInvestigatorMoveToPlaceTest : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator MoveToPlaceBasicTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, SceneCORE1.Hallway)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(SceneCORE1.Attic);

            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(GameValues.DEFAULT_TURNS_AMOUNT - SceneCORE1.Attic.MoveTurnsCost.Value));
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE1.Attic));
        }

        [UnityTest]
        public IEnumerator MoveToCard01115PlaceTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new RevealGameAction(SceneCORE1.Parlor)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, SceneCORE1.Hallway)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(SceneCORE1.Parlor);

            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(GameValues.DEFAULT_TURNS_AMOUNT - SceneCORE1.Parlor.MoveTurnsCost.Value));
            yield return ClickedMainButton();

            yield return gameActionTask.AsCoroutine();
            Assert.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE1.Parlor));
        }
    }
}
