using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01501Tests : TestBase
    {
        private int valueToken;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Investigator1StarToken()
        {
            _reactionableControl.SubscribeAtStart(RevealStarToken);
            CardPlace place = _cardsProvider.GetCard<Card01114>(); //Enigma:4, Hints: 2
            int valueTokenExpected = place.Hints.Value;
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01501>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            yield return _preparationScene.PlayThisInvestigator(investigatorToTest);
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(investigatorToTest.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();

            Task<PlayInvestigatorGameAction> taskInvestigator = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(place);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!taskInvestigator.IsCompleted) yield return null;
            Assert.That(valueToken, Is.EqualTo(valueTokenExpected));
        }

        private async Task RevealStarToken(GameAction gameAction)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
            ChallengeToken starToken = _challengeTokensProvider.ChallengeTokensInBag
               .Find(challengeToken => challengeToken.TokenType == ChallengeTokenType.Star);
            revealChallengeTokenGameAction.SetChallengeToken(starToken);
            valueToken = starToken.Value.Invoke();

            await Task.CompletedTask;
        }
    }
}
