﻿using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01513Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator PlaceEldritch()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01513 darkMemory = _cardsProvider.GetCard<Card01513>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(darkMemory, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(darkMemory);
            yield return ClickedMainButton();

            Assert.That(SceneCORE1.CurrentPlot.AmountDecrementedEldritch, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator TakeFear()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01513 darkMemory = _cardsProvider.GetCard<Card01513>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(darkMemory, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedMainButton();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(2));
        }
    }
}
