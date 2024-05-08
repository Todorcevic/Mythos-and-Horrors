using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01603Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator BlankCratureBuffTest()
        {
            yield return _preparationScene.StartingScene();
            CardCreature creature = _cardsProvider.GetCard<Card01603>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _preparationScene.SceneCORE1.Study.OwnZone)).AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Third));
            while (_gameActionsProvider.CurrentInteractable == null) yield return null;
            Assert.That(_investigatorsProvider.Third.InvestigatorCard.CanBePlayed, Is.False);

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();
            taskGameAction = _gameActionsProvider.Create(new DefeatCardGameAction(creature, _investigatorsProvider.First.InvestigatorCard));
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.InvestigatorCard);
            yield return taskGameAction.AsCoroutine();
            Assert.That(_investigatorsProvider.First.Hints.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator BlankCratureStarTokenBuffTest()
        {
            _reactionableControl.SubscribeAtStart(RevealStarToken);
            yield return _preparationScene.StartingScene();
            CardCreature creature = _cardsProvider.GetCard<Card01603>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _preparationScene.SceneCORE1.Study.OwnZone)).AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Third));
            if (!DEBUG_MODE) yield return WaitToClick(_preparationScene.SceneCORE1.Study);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            int? challengeValue = null;
            while (challengeValue == null)
            {
                challengeValue ??= _gameActionsProvider.CurrentChallenge?.TokensRevealed.First().Value.Invoke();
                yield return null;
            }

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(0));

            yield return _gameActionsProvider.Create(new DefeatCardGameAction(creature, _investigatorsProvider.Third.InvestigatorCard)).AsCoroutine();
            yield return _gameActionsProvider.Create(new ResetAllInvestigatorsTurnsGameAction()).AsCoroutine();
            taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Third));
            if (!DEBUG_MODE) yield return WaitToClick(_preparationScene.SceneCORE1.Study);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            challengeValue = null;
            while (challengeValue == null)
            {
                challengeValue ??= _gameActionsProvider.CurrentChallenge?.TokensRevealed.First().Value.Invoke();
                yield return null;
            }
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(2));
        }

        private async Task RevealStarToken(GameAction gameAction)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
            ChallengeToken minus1Token = _challengeTokensProvider.ChallengeTokensInBag
                .Find(challengeToken => challengeToken.TokenType == ChallengeTokenType.Star);
            revealChallengeTokenGameAction.SetChallengeToken(minus1Token);

            await Task.CompletedTask;
        }
    }
}
