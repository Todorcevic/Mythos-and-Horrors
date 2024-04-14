using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ActiveCardGameActionTest : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator ActivateCardGameActionTest()
        {
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.First);
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.Second);
            CardSupply aidCard = _cardsProvider.GetCard<Card01535>();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.Second.Health, 2)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.Second.CurrentTurns, _investigatorsProvider.Second.MaxTurns.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(aidCard, _investigatorsProvider.Second.AidZone)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(aidCard).AsTask();
            if (!DEBUG_MODE) WaitToClick(_investigatorsProvider.Second.AvatarCard).AsTask();
            yield return _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.Second)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.Second.Health.Value, Is.EqualTo(3));
        }
    }
}
