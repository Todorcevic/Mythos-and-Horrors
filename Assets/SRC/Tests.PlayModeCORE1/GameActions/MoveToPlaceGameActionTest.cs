using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class MoveToPlaceGameActionTest : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator MoveToPlaceTest()
        {
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(_preparationScene.SceneCORE1.Attic);

            Assert.That(_investigatorsProvider.First.CurrentTurns.Value, Is.EqualTo(GameValues.DEFAULT_TURNS_AMOUNT - _preparationScene.SceneCORE1.Attic.MoveTurnsCost.Value));
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return gameActionTask.AsCoroutine();
            Assert.That(_investigatorsProvider.First.CurrentPlace, Is.EqualTo(_preparationScene.SceneCORE1.Attic));
        }

        [UnityTest]
        public IEnumerator MoveToCard01115PlaceTest()
        {
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new RevealGameAction(_preparationScene.SceneCORE1.Parlor)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(_preparationScene.SceneCORE1.Parlor);

            Assert.That(_investigatorsProvider.First.CurrentTurns.Value, Is.EqualTo(GameValues.DEFAULT_TURNS_AMOUNT - _preparationScene.SceneCORE1.Parlor.MoveTurnsCost.Value));
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return gameActionTask.AsCoroutine();
            Assert.That(_investigatorsProvider.First.CurrentPlace, Is.EqualTo(_preparationScene.SceneCORE1.Parlor));
        }
    }
}
