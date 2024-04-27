using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01504Tests : TestBase
    {
        private int valueToken;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Investigator4StarToken()
        {
            _reactionableControl.SubscribeAtStart(RevealStarToken);
            CardPlace place = _cardsProvider.GetCard<Card01114>(); //Enigma:4, Hints: 2
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01504>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            yield return _preparationScene.PlayThisInvestigator(investigatorToTest);
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(investigatorToTest.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(investigatorToTest.Sanity, 3)).AsCoroutine();

            Task<PlayInvestigatorGameAction> taskInvestigator = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(place);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!taskInvestigator.IsCompleted) yield return null;
            Assert.That(valueToken, Is.EqualTo(investigatorToTest.FearRecived));
        }

        [UnityTest]
        public IEnumerator HarmCreatureWithSanityTest()
        {
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01504>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.GhoulSecuaz, investigatorToTest.CurrentPlace.OwnZone)).AsCoroutine();
            Task<HarmToInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigatorToTest, _preparationScene.SceneCORE1.GhoulSecuaz, amountFear: 3, isDirect: true));

            if (!DEBUG_MODE) yield return WaitToClick(_preparationScene.SceneCORE1.GhoulSecuaz);

            while (!taskGameAction.IsCompleted) yield return null;
            Assert.That(_preparationScene.SceneCORE1.GhoulSecuaz.Health.Value, Is.EqualTo(1));
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
