using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01502Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Investigator2StarToken()
        {
            _reactionableControl.SubscribeAtStart(RevealStarToken);
            Card tomeCard = _cardsProvider.GetCard<Card01531>();
            Card tomeCard2 = _cardsProvider.GetCard<Card01535>();
            Investigator investigatorToTest = _investigatorsProvider.Second;
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard, investigatorToTest.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard2, investigatorToTest.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, _preparationScene.SceneCORE1.Cellar)).AsCoroutine();
            int resultExpected = investigatorToTest.HandSize + 2;

            Task<PlayInvestigatorGameAction> taskInvestigator = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(_preparationScene.SceneCORE1.Cellar);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!taskInvestigator.IsCompleted) yield return null;
            Assert.That(investigatorToTest.HandSize, Is.EqualTo(resultExpected));
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
