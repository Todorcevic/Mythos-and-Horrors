using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigatorCard01503Tests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator StarChallengeTokenRevealed()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Star);
            Investigator investigatorToTest = _investigatorsProvider.Third;
            CardPlace place = _preparationSceneCORE1.SceneCORE1.Cellar;
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();
            int resutlExpected = investigatorToTest.Resources.Value + 2;

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            yield return ClickedIn(place);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.Resources.Value, Is.EqualTo(resutlExpected));
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator GainExtraTurn()
        {
            Investigator investigatorToTest = _investigatorsProvider.Third;
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigatorToTest, withResources: true);

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            yield return ClickedIn(investigatorToTest.InvestigatorCard);
            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(4));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.Resources.Value, Is.EqualTo(3));
        }
    }
}
