﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardPlot01106Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator DrawGhoulWhenComplete()
        {
            Card01106 cardPlot = _cardsProvider.GetCard<Card01106>();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.DangerCards, SceneCORE1.DangerDeckZone, isFaceDown: true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.DangerCards.Take(10), SceneCORE1.DangerDiscardZone, isFaceDown: false)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, SceneCORE1.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            yield return _gameActionsProvider.Create(new CheckEldritchsPlotGameAction()).AsCoroutine();

            Assert.That(SceneCORE1.DangerCards.Any(card => card.Tags.Contains(Tag.Ghoul) && card.IsInPlay), Is.True);
        }
    }
}