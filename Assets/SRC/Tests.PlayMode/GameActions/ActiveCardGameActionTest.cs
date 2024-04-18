using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(aidCard, _investigatorsProvider.Second.AidZone)).AsCoroutine();
            Dictionary<Stat, int> stats = new()
            {
                { _investigatorsProvider.Second.Health, 2 },
                { _investigatorsProvider.First.Health, 4},
                { _investigatorsProvider.Second.CurrentTurns, _investigatorsProvider.Second.MaxTurns.Value }
            };
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(stats)).AsCoroutine();

            _ = _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.AvatarCard);
            if (!DEBUG_MODE) yield return WaitToClick(aidCard);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.AvatarCard);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.Second.Health.Value, Is.EqualTo(3));
        }
    }
}
