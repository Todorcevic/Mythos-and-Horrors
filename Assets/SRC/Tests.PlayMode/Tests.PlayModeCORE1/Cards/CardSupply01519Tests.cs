﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01519Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ActivateToHealInvestigator()
        {
            yield return StartingScene(withResources: true);

            Card01519 supplyCard = _cardsProvider.GetCard<Card01519>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supplyCard, _investigatorsProvider.First.AidZone).Execute().AsCoroutine();
            Dictionary<Stat, int> stats = new()
            {
                { _investigatorsProvider.First.DamageRecived, 4},
                { _investigatorsProvider.First.FearRecived, 2},
                { _investigatorsProvider.Second.DamageRecived, 2},
                { _investigatorsProvider.Second.FearRecived, 2},
                { _investigatorsProvider.Third.DamageRecived, 1},
                { _investigatorsProvider.Fourth.FearRecived, 1}
            };
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(stats).Execute().AsCoroutine();
            Assert.That(supplyCard.Charge.Amount.Value, Is.EqualTo(3));

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.First).Execute();
            yield return ClickedIn(supplyCard);
            yield return ClickedClone(_investigatorsProvider.Second.InvestigatorCard, 0);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Second.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(supplyCard.Charge.Amount.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator DiscardIfNotSupplies()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            Card01519 supplyCard = _cardsProvider.GetCard<Card01519>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supplyCard, _investigatorsProvider.First.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(investigator.DamageRecived, 1).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(supplyCard.Charge.Amount, 1).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.First).Execute();
            yield return ClickedIn(supplyCard);
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(0));
            Assert.That(supplyCard.CurrentZone, Is.EqualTo(investigator.DiscardZone));
        }
    }
}
