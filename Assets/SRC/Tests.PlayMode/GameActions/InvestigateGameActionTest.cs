﻿using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigateGameActionTest : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InvestigateTest()
        {
            _prepareGameUseCase.Execute();
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01111");
            yield return _gameActionFactory.Create(new MoveCardsGameAction(place, _chapterProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
            if (!DEBUG_MODE) WaitToClickHistoryPanel().AsTask();
            yield return _gameActionFactory.Create(new MoveToPlaceGameAction(_investigatorsProvider.Leader, place)).AsCoroutine();

            yield return _gameActionFactory.Create(new InvestigateGameAction(_investigatorsProvider.Leader, place)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator RealInvestigateTest()
        {
            _prepareGameUseCase.Execute();
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01111");
            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.Turns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(place, _chapterProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
            if (!DEBUG_MODE) WaitToClickHistoryPanel().AsTask();
            yield return _gameActionFactory.Create(new MoveToPlaceGameAction(_investigatorsProvider.Leader, place)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(place).AsTask();
            yield return _gameActionFactory.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.Leader)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.Leader.Turns.Value, Is.EqualTo(GameValues.DEFAULT_TURNS_AMOUNT - place.InvestigationCost.Value));
            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(1));
        }
    }
}