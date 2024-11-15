﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01552Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Dodamage()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01552 conditionCard = _cardsProvider.GetCard<Card01552>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(SceneCORE1.GhoulGelid.Exausted, true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulGelid, investigator.CurrentPlace).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.CurrentPlace).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(SceneCORE1.GhoulGelid);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(SceneCORE1.GhoulGelid.DamageRecived.Value, Is.EqualTo(2));
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }
    }
}
