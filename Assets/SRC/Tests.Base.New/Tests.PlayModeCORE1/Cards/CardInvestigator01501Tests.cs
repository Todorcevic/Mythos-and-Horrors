﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CardInvestigator01501Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator StarChallengeTokenRevealed()
        {
            CardPlace place = _cardsProvider.GetCard<Card01114>();
            Investigator investigatorToTest = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Star);
            Task<int> tokenValue = CaptureTokenValue(investigatorToTest);
            yield return PlayThisInvestigator(investigatorToTest, withAvatar: false);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            yield return ClickedIn(place);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(8));
        }

        [UnityTest]
        public IEnumerator DiscoverClueWhenDefeatCreature()
        {
            Investigator investigatorToTest = _investigatorsProvider.First;
            CardCreature cardCreature = SceneCORE1.GhoulSecuaz;
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardCreature, investigatorToTest.DangerZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new HarmToCardGameAction(cardCreature, investigatorToTest.InvestigatorCard, amountDamage: 5));
            yield return ClickedIn(investigatorToTest.InvestigatorCard);
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.Hints.Value, Is.EqualTo(1));
        }
    }
}