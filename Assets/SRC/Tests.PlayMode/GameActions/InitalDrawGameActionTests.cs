﻿using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{

    public class InitalDrawGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InitialDrawGameAction()
        {
            _prepareGameUseCase.Execute();
            Investigator investigator = _investigatorsProvider.AllInvestigators[0];
            Card card = _cardsProvider.GetCard("01517");
            yield return _gameActionFactory.Create(new MoveCardsGameAction(card, investigator.DeckZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new InitialDrawGameAction(investigator)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(investigator.HandZone.Cards.Contains(card));
            yield return null;
        }

        [UnityTest]
        public IEnumerator InitialDrawWeaknessGameAction()
        {
            _prepareGameUseCase.Execute();
            Investigator investigator = _investigatorsProvider.AllInvestigators[0];
            Card weaknessCard = _cardsProvider.GetCard("01507");
            Card normalCard = _cardsProvider.GetCard("01517");
            yield return _gameActionFactory.Create(new MoveCardsGameAction(new System.Collections.Generic.List<Card>() { normalCard, weaknessCard }, investigator.DeckZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new InitialDrawGameAction(investigator)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(investigator.HandZone.Cards.Contains(normalCard));
            Assert.That(investigator.DiscardZone.Cards.Contains(weaknessCard));
            yield return null;
        }
    }
}