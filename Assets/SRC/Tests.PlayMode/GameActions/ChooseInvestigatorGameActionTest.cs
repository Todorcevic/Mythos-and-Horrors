using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
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
            yield return _gameActionsProvider.Create(new ResetAllInvestigatorsTurnsGameAction()).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, place)).AsCoroutine();

            _ = _gameActionsProvider.Create(new ChooseInvestigatorGameAction());
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.AvatarCard);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_gameActionsProvider.CurrentPlayInvestigatorPhaseInvestigator.ActiveInvestigator, Is.EqualTo(_investigatorsProvider.Second));
        }
    }
}
