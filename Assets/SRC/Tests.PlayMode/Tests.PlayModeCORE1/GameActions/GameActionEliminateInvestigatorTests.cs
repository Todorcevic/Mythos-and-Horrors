﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionEliminateInvestigatorTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator EliminateInvestigatorTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardAdversity adversity = _cardsProvider.GetCard<Card01162>();
            CardCreature ghoulSecuaz = SceneCORE1.GhoulSecuaz;
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(ghoulSecuaz, investigator.DangerZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversity, investigator.DangerZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(investigator, SceneCORE1.Attic.Keys, 2).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, ghoulSecuaz, amountFear: 8).Execute().AsCoroutine();

            Assert.That(investigator.Defeated.IsActive, Is.True);
            Assert.That(adversity.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
            Assert.That(ghoulSecuaz.CurrentPlace, Is.EqualTo(SceneCORE1.Study));
            Assert.That(SceneCORE1.Study.Keys.Value, Is.EqualTo(10));
            Assert.That(investigator.Defeated.IsActive, Is.True);
        }
    }
}