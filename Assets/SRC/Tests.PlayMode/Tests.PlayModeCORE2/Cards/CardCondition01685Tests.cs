﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCondition01685Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator MultiInvestigation()
        {
            Investigator investigator = _investigatorsProvider.Second;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return BuildCard("01685", investigator);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01685 conditionCard = _cardsProvider.GetCard<Card01685>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator.HandZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(conditionCard);
            yield return ClickedMainButton();
            yield return ClickedIn(SceneCORE2.Fluvial);
            yield return ClickedIn(SceneCORE2.East);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(2));
        }
    }
}