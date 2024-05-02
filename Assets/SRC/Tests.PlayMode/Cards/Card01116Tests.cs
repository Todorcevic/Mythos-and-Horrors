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
            _reactionableControl.SubscribeAtStart((gameAction) => RevealSpecificToken(gameAction, ChallengeTokenType.Value_3));
            yield return _preparationScene.PlaceAllSceneCards();
            yield return _preparationScene.PlaceAllPlaceCards();
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.GhoulPriest, _investigatorsProvider.First.DangerZone)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(_preparationScene.SceneCORE1.GhoulPriest);
            if (!DEBUG_MODE) yield return WaitToCloneClick(0);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!gameActionTask.IsCompleted) yield return null;

            Assert.That(_investigatorsProvider.First.DamageRecived, Is.EqualTo(2));
            Assert.That(_investigatorsProvider.First.FearRecived, Is.EqualTo(2));
        }

        private async Task RevealSpecificToken(GameAction gameAction, ChallengeTokenType tokenType)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
            ChallengeToken token = _challengeTokensProvider.ChallengeTokensInBag
                .Find(challengeToken => challengeToken.TokenType == tokenType);
            revealChallengeTokenGameAction.SetChallengeToken(token);

            await Task.CompletedTask;
        }
    }
}
