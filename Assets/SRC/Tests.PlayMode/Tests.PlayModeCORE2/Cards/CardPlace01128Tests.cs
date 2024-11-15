﻿using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardPlace01128Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator HealthDamage()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01128 cardPlace = _cardsProvider.GetCard<Card01128>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, cardPlace).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, cardPlace, amountDamage: 2).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(cardPlace, 1);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(0));
        }
    }
}

