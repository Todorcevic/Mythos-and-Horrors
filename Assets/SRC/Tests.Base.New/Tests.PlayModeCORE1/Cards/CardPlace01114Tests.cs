﻿using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Linq;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CardPlace01114Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator TakeDamageInvestigatorWhenEnter()
        {
            CardPlace Cellar = _cardsProvider.GetCard<Card01114>();
            Investigator investigator = _investigatorsProvider.AllInvestigators.First();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Cellar, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, Cellar)).AsCoroutine();

            Assert.That(investigator.DamageRecived, Is.EqualTo(1));
        }
    }
}