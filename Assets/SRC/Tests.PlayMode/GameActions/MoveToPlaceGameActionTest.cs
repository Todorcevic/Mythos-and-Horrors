using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

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

            if (!DEBUG_MODE) WaitToClick(_preparationScene.SceneCORE1.Attic).AsTask();
            yield return _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.First)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.First.CurrentTurns.Value, Is.EqualTo(GameValues.DEFAULT_TURNS_AMOUNT - _preparationScene.SceneCORE1.Attic.MoveTurnsCost.Value));
            Assert.That(_investigatorsProvider.First.CurrentPlace, Is.EqualTo(_preparationScene.SceneCORE1.Attic));
        }

        [UnityTest]
        public IEnumerator MoveToCard01115PlaceTest()
        {
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new RevealGameAction(_preparationScene.SceneCORE1.Parlor)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(_preparationScene.SceneCORE1.Parlor).AsTask();
            yield return _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.First)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.First.CurrentTurns.Value, Is.EqualTo(GameValues.DEFAULT_TURNS_AMOUNT - _preparationScene.SceneCORE1.Parlor.MoveTurnsCost.Value));
            Assert.That(_investigatorsProvider.First.CurrentPlace, Is.EqualTo(_preparationScene.SceneCORE1.Parlor));
        }
    }
}
