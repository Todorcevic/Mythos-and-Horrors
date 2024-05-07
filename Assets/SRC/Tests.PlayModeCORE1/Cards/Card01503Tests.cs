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
            _reactionableControl.SubscribeAtStart(RevealStarToken);
            Investigator investigatorToTest = _investigatorsProvider.Third;
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, _preparationScene.SceneCORE1.Cellar)).AsCoroutine();
            int resutlExpected = investigatorToTest.Resources.Value + 2;

            Task<PlayInvestigatorGameAction> taskInvestigator = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(_preparationScene.SceneCORE1.Cellar);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!taskInvestigator.IsCompleted) yield return null;
            Assert.That(investigatorToTest.Resources.Value, Is.EqualTo(resutlExpected));
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Investigator3GainTurnTest()
        {
            Investigator investigatorToTest = _investigatorsProvider.Third;
            yield return _preparationScene.StartingScene();

            Task<PlayInvestigatorGameAction> taskInvestigator = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(investigatorToTest.InvestigatorCard);
            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(4));

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            while (!taskInvestigator.IsCompleted) yield return null;
            Assert.That(investigatorToTest.Resources.Value, Is.EqualTo(3));
        }

        private async Task RevealStarToken(GameAction gameAction)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
            ChallengeToken starToken = _challengeTokensProvider.ChallengeTokensInBag
               .Find(challengeToken => challengeToken.TokenType == ChallengeTokenType.Star);
            revealChallengeTokenGameAction.SetChallengeToken(starToken);

            await Task.CompletedTask;
        }
    }
}
