using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class ResourceGameActionTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Full_Take_Resource()
        {
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.First);
            //yield return new WaitForSeconds(10);
            WaitToTokenClick().AsTask();
            yield return _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.First)).AsCoroutine();


            //while (!gameActionTask.IsCompleted) yield return null;
            if (DEBUG_MODE) yield return PressAnyKey();

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.First).ResourcesTokenController.Amount, Is.EqualTo(6));
            Assert.That(_investigatorsProvider.First.Resources.Value, Is.EqualTo(6));
        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Investigator()
        {
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.First);
            do
            {
                yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.First, 5)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.First).ResourcesTokenController.Amount, Is.EqualTo(10));
        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Pile()
        {
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.First);
            do
            {
                yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.First, 5)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();
                yield return _gameActionsProvider.Create(new PayResourceGameAction(_investigatorsProvider.First, 2)).AsCoroutine();

                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.First).ResourcesTokenController.Amount, Is.EqualTo(8));
        }

        [UnityTest]
        public IEnumerator Move_Resource_Swaping()
        {
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.First);
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.Second);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.Cards[0], _investigatorsProvider.First.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.Cards[0], _investigatorsProvider.Second.AidZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.First, 5)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.Second, 5)).AsCoroutine();

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.Second).ResourcesTokenController.Amount, Is.EqualTo(10));
        }
    }
}
