using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ChooseInvestigatorGameActionTest : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator ChooseInvestigatorTest()
        {
            yield return _preparationScene.PlayAllInvestigators();
            CardPlace place = _cardsProvider.GetCard<Card01111>();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.Second.CurrentTurns, 1)).AsCoroutine(); yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, place)).AsCoroutine();

            Task<ChooseInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new ChooseInvestigatorGameAction());
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.AvatarCard);
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            yield return gameActionTask.AsCoroutine();
            Assert.That(_investigatorsProvider.Second.CurrentTurns.Value, Is.EqualTo(0));
        }
    }
}
