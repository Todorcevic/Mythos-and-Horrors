﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CardInvestigator01503Tests : TestCORE1Preparation
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator StarChallengeTokenRevealed()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Star);
            Investigator investigatorToTest = _investigatorsProvider.Third;
            CardPlace place = SceneCORE1.Cellar;
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();
            int resutlExpected = investigatorToTest.Resources.Value + 2;

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            yield return ClickedIn(place);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.Resources.Value, Is.EqualTo(resutlExpected));
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator GainExtraTurn()
        {
            Investigator investigatorToTest = _investigatorsProvider.Third;
            yield return PlayThisInvestigator(investigatorToTest, withResources: true);

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            yield return ClickedIn(investigatorToTest.InvestigatorCard);
            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(4));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.Resources.Value, Is.EqualTo(3));
        }
    }
}