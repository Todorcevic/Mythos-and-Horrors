﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionPrepareInvestigatorTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator PermanentAtStart()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01694", investigator);
            Card01694 supply = _cardsProvider.GetCard<Card01694>();

            yield return PlaceOnlyScene();
            Task taskGameAction = _gameActionsProvider.Create<PrepareInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.IsInPlay, Is.True);

        }
    }
}