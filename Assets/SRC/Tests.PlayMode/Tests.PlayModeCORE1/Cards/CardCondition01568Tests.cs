﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{

    public class CardCondition01568Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator BlankCreature()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuilCard("01568", investigator);
            Card01568 conditionCard = _cardsProvider.GetCard<Card01568>();
            Card01603 creature = _cardsProvider.GetCard<Card01603>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(creature, investigator.CurrentPlace)).AsCoroutine();

            Assert.That(investigator.InvestigatorCard.Blancked.IsActive, Is.True);

            Task gameActionTask = _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(creature);
            Assert.That(investigator.InvestigatorCard.Blancked.IsActive, Is.False);
            Assert.That(creature.Blancked.IsActive, Is.True);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.InvestigatorCard.Blancked.IsActive, Is.True);
            Assert.That(creature.Blancked.IsActive, Is.False);
        }
    }
}