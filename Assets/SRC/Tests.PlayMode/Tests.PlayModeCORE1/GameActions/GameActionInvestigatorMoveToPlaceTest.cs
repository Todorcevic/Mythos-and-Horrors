using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionInvestigatorMoveToPlaceTest : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator MoveToPlaceBasicTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(SceneCORE1.Attic);

            Assert.That(investigator.CurrentActions.Value, Is.EqualTo(2));
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE1.Attic));
        }

        [UnityTest]
        public IEnumerator MoveToCard01115PlaceTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            yield return _gameActionsProvider.Create<RevealGameAction>().SetWith(SceneCORE1.Parlor).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(SceneCORE1.Parlor);

            Assert.That(investigator.CurrentActions.Value, Is.EqualTo(2));
            yield return ClickedMainButton();

            yield return gameActionTask.AsCoroutine();
            Assert.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE1.Parlor));
        }
    }
}
