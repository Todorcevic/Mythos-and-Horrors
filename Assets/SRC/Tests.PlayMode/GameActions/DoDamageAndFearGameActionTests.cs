﻿using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class DoDamageAndFearGameActionTests : TestBase
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardLoaderUseCase _cardLoaderUseCase;
        [Inject] private readonly CardViewGeneratorComponent _cardViewGeneratorComponent;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator DamageTest()
        {
            Card bulletProof = _cardLoaderUseCase.Execute("01594");
            _cardViewGeneratorComponent.BuildCardView(bulletProof);
            Investigator investigator = _investigatorsProvider.First;
            Card damageableCard = investigator.AllCards.First(card => card.Info.Code == "01521");
            Card damageableCard2 = investigator.AllCards.First(card => card.Info.Code == "01521" && card != damageableCard);
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard2, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(bulletProof, investigator.AidZone)).AsCoroutine();

            Task<HarmToInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, amountDamage: 2, amountFear: 1));
            if (!DEBUG_MODE) yield return WaitToClick(bulletProof);
            if (!DEBUG_MODE) yield return WaitToClick(damageableCard);
            while (!gameActionTask.IsCompleted) yield return null;

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(investigator.AidZone.Cards.Count, Is.EqualTo(2));
            Assert.That(((IDamageable)bulletProof).Health.Value, Is.EqualTo(2));
        }
    }
}