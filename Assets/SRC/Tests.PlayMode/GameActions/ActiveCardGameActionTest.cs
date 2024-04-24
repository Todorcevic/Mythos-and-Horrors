using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            yield return _preparationScene.StartingScene();

            CardSupply aidCard = _cardsProvider.GetCard<Card01535>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(aidCard, _investigatorsProvider.Second.AidZone)).AsCoroutine();
            Dictionary<Stat, int> stats = new()
            {
                { _investigatorsProvider.Second.Health, 2 },
                { _investigatorsProvider.First.Health, 4}
            };
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(stats)).AsCoroutine();

            Task<OneInvestigatorTurnGameAction> taskGameAction = _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.Second));
            if (!DEBUG_MODE) yield return WaitToClick(aidCard);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.AvatarCard);

            while (!taskGameAction.IsCompleted) yield return null;
            if (DEBUG_MODE) yield return PressAnyKey();

            Assert.That(_investigatorsProvider.Second.Health.Value, Is.EqualTo(3));
        }
    }
}
