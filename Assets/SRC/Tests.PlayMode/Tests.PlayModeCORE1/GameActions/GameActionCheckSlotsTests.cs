﻿using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionCheckSlotsTests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CheckSlots()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card toPlay = _cardsProvider.GetCard<Card01518>();
            Card toPlay2 = _cardsProvider.GetCard<Card01521>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay2, investigator.HandZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(toPlay2);
            yield return ClickedIn(toPlay);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.AidZone.Cards.Contains(toPlay2), Is.True);
            Assert.That(investigator.DiscardZone.Cards.Contains(toPlay), Is.True);
        }
    }
}