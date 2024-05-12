using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01501Tests : TestCORE1PlayModeBase
    {
        private int valueToken;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Investigator1StarToken()
        {
            MustBeRevealedThisToken(ChallengeTokenType.Star);
            CardPlace place = _cardsProvider.GetCard<Card01114>(); //Enigma:4, Hints: 2
            int valueTokenExpected = place.Hints.Value;
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01501>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigatorToTest);
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(investigatorToTest.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(place);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge == null) yield return null;
            int valueToken = _gameActionsProvider.CurrentChallenge.TokensRevealed.First().Value();

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return taskGameAction.AsCoroutine();
            Assert.That(valueToken, Is.EqualTo(valueTokenExpected));
        }

        [UnityTest]
        public IEnumerator Investigator1DiscoverClue()
        {
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01501>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz, investigatorToTest.DangerZone)).AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create(new HarmToCardGameAction(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz, investigatorToTest.InvestigatorCard, amountDamage: 5));
            if (!DEBUG_MODE) yield return WaitToClick(cardInvestigator);

            yield return taskGameAction.AsCoroutine();
            Assert.That(investigatorToTest.Hints.Value, Is.EqualTo(1));
        }
    }
}
