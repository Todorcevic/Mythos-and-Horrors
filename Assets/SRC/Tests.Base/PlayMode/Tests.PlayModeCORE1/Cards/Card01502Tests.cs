﻿using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01502Tests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Investigator2StarToken()
        {
            MustBeRevealedThisToken(ChallengeTokenType.Star);
            Card tomeCard = _cardsProvider.GetCard<Card01531>();
            Card tomeCard2 = _cardsProvider.GetCard<Card01535>();
            Investigator investigatorToTest = _investigatorsProvider.Second;
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard, investigatorToTest.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard2, investigatorToTest.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, _preparationSceneCORE1.SceneCORE1.Cellar)).AsCoroutine();
            int resultExpected = investigatorToTest.DeckZone.Cards.Count - 2;

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(_preparationSceneCORE1.SceneCORE1.Cellar);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return taskGameAction.AsCoroutine();
            Assert.That(investigatorToTest.DeckZone.Cards.Count, Is.EqualTo(resultExpected));
        }

        [UnityTest]
        public IEnumerator ExtraTurnToTomeTest()
        {
            Card tomeCard = _cardsProvider.GetCard<Card01535>();
            Investigator investigatorToTest = _investigatorsProvider.Second;
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard, investigatorToTest.AidZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            if (!DEBUG_MODE) yield return WaitToClick(investigatorToTest.InvestigatorCard);
            if (!DEBUG_MODE) yield return WaitToClick(tomeCard);
            if (!DEBUG_MODE) yield return WaitToClick(investigatorToTest.AvatarCard);

            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(3));

            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            yield return taskGameAction.AsCoroutine();
            Assert.That(investigatorToTest.Resources.Value, Is.EqualTo(3));
        }
    }
}