using DG.Tweening;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ChallengeTests : TestBase
    {
        protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PushTokenTest()
        {
            ChallengeToken challengeToken = new(ChallengeTokenType.Ancient, () => -2);
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.First);

            do
            {
                if (DEBUG_MODE) yield return PressAnyKey();

                yield return _challengeBagComponent.DropToken(challengeToken).AsCoroutine();
            } while (DEBUG_MODE);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_challengeBagComponent.GetPrivateMember<List<ChallengeTokenView>>("_allTokensDrop").Unique().ChallengeToken,
                Is.EqualTo(challengeToken));

            yield return _challengeBagComponent.RestoreToken(challengeToken).WaitForCompletion();

            Assert.That(_challengeBagComponent.GetPrivateMember<List<ChallengeTokenView>>("_allTokensDrop").Count, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator ChallengeWithCommitsTests()
        {
            _reactionableControl.SubscribeAtStart(RevealMinus1Token);
            yield return _preparationScene.StartingScene();
            Card toPlay = _cardsProvider.GetCard<Card01538>();
            Card toPlay2 = _cardsProvider.GetCard<Card01522>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay, _investigatorsProvider.Leader.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay2, _investigatorsProvider.Leader.HandZone)).AsCoroutine();

            Task<PlayInvestigatorLoopGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorLoopGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!gameActionTask.IsCompleted) yield return null;
            Assert.That(_investigatorsProvider.First.CurrentTurns.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.First.Hints.Value, Is.EqualTo(1));
        }

        private async Task RevealMinus1Token(GameAction gameAction)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
            ChallengeToken minus1Token = _challengeTokensProvider.ChallengeTokensInBag
                .Find(challengeToken => challengeToken.TokenType == ChallengeTokenType.Value_1);
            revealChallengeTokenGameAction.SetChallengeToken(minus1Token);

            await Task.CompletedTask;
        }

        private async Task SuccessEffect()
        {
            await _gameActionsProvider.Create(new DrawAidGameAction(_investigatorsProvider.Leader));
        }

        private async Task FailEffect()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(_investigatorsProvider.Leader.Health, 1));
        }
    }
}
