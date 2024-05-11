using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01503Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Investigator3StarTokenTest()
        {
            MustBeRevealedThisToken(ChallengeTokenType.Star);
            Investigator investigatorToTest = _investigatorsProvider.Third;
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, _preparationSceneCORE1.SceneCORE1.Cellar)).AsCoroutine();
            int resutlExpected = investigatorToTest.Resources.Value + 2;

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(_preparationSceneCORE1.SceneCORE1.Cellar);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return taskGameAction.AsCoroutine();
            Assert.That(investigatorToTest.Resources.Value, Is.EqualTo(resutlExpected));
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Investigator3GainTurnTest()
        {
            Investigator investigatorToTest = _investigatorsProvider.Third;
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigatorToTest, withResources: true);

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(investigatorToTest.InvestigatorCard);
            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(4));

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();
            Assert.That(investigatorToTest.Resources.Value, Is.EqualTo(3));
        }
    }
}
