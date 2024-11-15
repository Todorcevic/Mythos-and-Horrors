﻿using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01508Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CheckExtraSlots()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card toPlay = _cardsProvider.GetCard<Card01508>();
            IEnumerable<Card01535> cardsToPlay = _cardsProvider.GetCards<Card01535>();
            IEnumerable<Card01531> cardsToPlay2 = _cardsProvider.GetCards<Card01531>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<GainResourceGameAction>().SetWith(investigator, 5).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardsToPlay, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardsToPlay2, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(toPlay, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(toPlay);
            yield return ClickedIn(cardsToPlay2.ElementAt(0));
            yield return ClickedIn(cardsToPlay2.ElementAt(1));
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(cardsToPlay2.ElementAt(0).CurrentZone, Is.EqualTo(investigator.AidZone));
            Assert.That(cardsToPlay2.ElementAt(1).CurrentZone, Is.EqualTo(investigator.AidZone));
        }

        [UnityTest]
        public IEnumerator CheckExtraRemovingBagSlots()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card toPlay = _cardsProvider.GetCard<Card01508>();
            IEnumerable<Card01535> cardsToPlay = _cardsProvider.GetCards<Card01535>();
            IEnumerable<Card01531> cardsToPlay2 = _cardsProvider.GetCards<Card01531>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<GainResourceGameAction>().SetWith(investigator, 5).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardsToPlay, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardsToPlay2, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(toPlay, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(toPlay);
            yield return ClickedIn(cardsToPlay2.First());
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Task gameActionTask2 = _gameActionsProvider.Create<DiscardGameAction>().SetWith(toPlay).Execute();
            yield return ClickedIn(cardsToPlay.First());
            yield return gameActionTask2.AsCoroutine();

            Assert.That(cardsToPlay.First().CurrentZone, Is.EqualTo(investigator.DiscardZone));
        }
    }
}
