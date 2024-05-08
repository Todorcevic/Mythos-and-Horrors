using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01504Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Investigator4StarToken()
        {
            RevealToken(ChallengeTokenType.Star);
            CardPlace place = _cardsProvider.GetCard<Card01114>(); //Enigma:4, Hints: 2
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01504>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            yield return _preparationScene.PlayThisInvestigator(investigatorToTest);
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(investigatorToTest.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(investigatorToTest.Sanity, 3)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(place);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            while (_gameActionsProvider.CurrentChallenge == null) yield return null;
            int valueToken = _gameActionsProvider.CurrentChallenge.TokensRevealed.First().Value();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return taskGameAction.AsCoroutine();
            Assert.That(valueToken, Is.EqualTo(investigatorToTest.FearRecived));
        }

        [UnityTest]
        public IEnumerator HarmCreatureWithSanityTest()
        {
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01504>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.GhoulSecuaz, investigatorToTest.CurrentPlace.OwnZone)).AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigatorToTest, _preparationScene.SceneCORE1.GhoulSecuaz, amountFear: 3, isDirect: true));

            if (!DEBUG_MODE) yield return WaitToClick(cardInvestigator);
            if (!DEBUG_MODE) yield return WaitToClick(_preparationScene.SceneCORE1.GhoulSecuaz);

            yield return taskGameAction.AsCoroutine();
            Assert.That(_preparationScene.SceneCORE1.GhoulSecuaz.Health.Value, Is.EqualTo(1));
        }
    }
}
