using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01116Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RetiliateTest()
        {
            RevealToken(ChallengeTokenType.Value_3);
            yield return _preparationScene.PlaceAllSceneCORE1();
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.GhoulPriest, _investigatorsProvider.First.DangerZone)).AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(_preparationScene.SceneCORE1.GhoulPriest);
            if (!DEBUG_MODE) yield return WaitToCloneClick(0);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.First.DamageRecived, Is.EqualTo(2));
            Assert.That(_investigatorsProvider.First.FearRecived, Is.EqualTo(2));
        }
    }
}
