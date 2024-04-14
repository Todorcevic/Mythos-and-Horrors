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
            CardPlace place = _cardsProvider.GetCard<Card01114>(); //Enigma:4, Hints: 2
            Card tomeCard = _cardsProvider.GetCard<Card01531>();
            Card tomeCard2 = _cardsProvider.GetCard<Card01535>();
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01502>();
            Investigator investigatorToTest = cardInvestigator.Owner;

            yield return _preparationScene.PlayThisInvestigator(investigatorToTest);
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(investigatorToTest.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard, investigatorToTest.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard2, investigatorToTest.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();


            Task<OneInvestigatorTurnGameAction> taskInvestigator =
             _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(place);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!taskInvestigator.IsCompleted) yield return null;
            Assert.That(investigatorToTest.HandSize, Is.EqualTo(2));
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
