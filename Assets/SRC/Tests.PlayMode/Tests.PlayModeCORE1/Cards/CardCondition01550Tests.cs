﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01550Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DeconfrontAndMove()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01550 conditionCard = _cardsProvider.GetCard<Card01550>();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(SceneCORE1.GhoulSecuaz, investigator.CurrentPlace)).AsCoroutine();
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(SceneCORE1.GhoulVoraz, SceneCORE1.Attic)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(conditionCard);
            Assert.That(SceneCORE1.Attic.CanBePlayed, Is.False);
            Assert.That(SceneCORE1.Cellar.CanBePlayed, Is.True);
            yield return ClickedIn(SceneCORE1.Cellar);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE1.Cellar));
            Assert.That(SceneCORE1.GhoulSecuaz.CurrentPlace, Is.EqualTo(SceneCORE1.Hallway));
        }
    }
}