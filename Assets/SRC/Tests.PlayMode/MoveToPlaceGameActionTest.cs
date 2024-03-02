using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class MoveToPlaceGameActionTest : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator MoveToPlaceTest()
        {
            _prepareGameUseCase.Execute();
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01112");
            CardPlace place2 = _cardsProvider.GetCard<CardPlace>("01113");

            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.Turns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(place, _chapterProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(place2, _chapterProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
            if (!DEBUG_MODE) WaitToClickHistoryPanel().AsTask();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.AvatarCard, place.OwnZone)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(place2).AsTask();
            if (!DEBUG_MODE) WaitToClickHistoryPanel().AsTask();
            yield return _gameActionFactory.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.Leader)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.Leader.Turns.Value, Is.EqualTo(GameValues.DEFAULT_TURNS_AMOUNT - place.MoveCost.Value));
            Assert.That(_investigatorsProvider.Leader.CurrentPlace, Is.EqualTo(place2));
        }

        [UnityTest]
        public IEnumerator MoveToCard01115PlaceTest()
        {
            _prepareGameUseCase.Execute();
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01112");
            CardPlace place2 = _cardsProvider.GetCard<CardPlace>("01115");

            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.Turns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(place, _chapterProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(place2, _chapterProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
            if (!DEBUG_MODE) WaitToClickHistoryPanel().AsTask();
            yield return _gameActionFactory.Create(new RevealGameAction(place2)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClickHistoryPanel().AsTask();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.AvatarCard, place.OwnZone)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(place2).AsTask();
            yield return _gameActionFactory.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.Leader)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.Leader.Turns.Value, Is.EqualTo(GameValues.DEFAULT_TURNS_AMOUNT - place.MoveCost.Value));
            Assert.That(_investigatorsProvider.Leader.CurrentPlace, Is.EqualTo(place2));
        }
    }
}
