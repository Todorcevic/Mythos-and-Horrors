using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01504Tests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Investigator4StarToken()
        {
            MustBeRevealedThisToken(ChallengeTokenType.Star);
            CardPlace place = _cardsProvider.GetCard<Card01114>(); //Enigma:4, Hints: 2
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01504>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigatorToTest);
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(investigatorToTest.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(investigatorToTest.Sanity, 3)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(place);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            while (_gameActionsProvider.CurrentChallenge == null) yield return null;
            int valueToken = _gameActionsProvider.CurrentChallenge.TokensRevealed.First().Value(investigatorToTest);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return taskGameAction.AsCoroutine();
            Assert.That(valueToken, Is.EqualTo(investigatorToTest.FearRecived));
        }

        [UnityTest]
        public IEnumerator HarmCreatureWithSanityTest()
        {
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01504>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz, investigatorToTest.CurrentPlace.OwnZone)).AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigatorToTest, _preparationSceneCORE1.SceneCORE1.GhoulSecuaz, amountFear: 3, isDirect: true));

            if (!DEBUG_MODE) yield return WaitToClick(cardInvestigator);
            if (!DEBUG_MODE) yield return WaitToClick(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz);

            yield return taskGameAction.AsCoroutine();
            Assert.That(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz.Health.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator OpportunityAttackAnyCreatureWithSafeForeachAndEliminate()
        {
            CardPlace place = _cardsProvider.GetCard<Card01111>();
            Investigator investigator = _investigatorsProvider.Fourth;
            CardCreature creature = _preparationSceneCORE1.SceneCORE1.GhoulSecuaz;
            CardCreature creature2 = _preparationSceneCORE1.SceneCORE1.GhoulVoraz;
            MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, place)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature2, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new HarmToCardGameAction(creature2, investigator.InvestigatorCard, amountDamage: 2)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(place);
            if (!DEBUG_MODE) yield return WaitToClick(investigator.InvestigatorCard);
            if (!DEBUG_MODE) yield return WaitToClick(investigator.InvestigatorCard);
            if (!DEBUG_MODE) yield return WaitToClick(creature2);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DamageRecived, Is.EqualTo(1));
            Assert.That(investigator.FearRecived, Is.EqualTo(1));
        }
    }
}
