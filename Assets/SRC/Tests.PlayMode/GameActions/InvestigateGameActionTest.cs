using MythosAndHorrors.GameRules;
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
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InvestigateTest()
        {
            _prepareGameUseCase.Execute();
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01111");
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chapterProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
            if (!DEBUG_MODE) WaitToHistoryPanelClick().AsTask();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, place)).AsCoroutine();

            yield return _gameActionsProvider.Create(new InvestigateGameAction(_investigatorsProvider.First, place)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.First.Hints.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator RealInvestigateTest()
        {
            _prepareGameUseCase.Execute();
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01111");
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chapterProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
            if (!DEBUG_MODE) WaitToHistoryPanelClick().AsTask();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, place)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(place).AsTask();
            yield return _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.First)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.First.CurrentTurns.Value, Is.EqualTo(GameValues.DEFAULT_TURNS_AMOUNT - place.InvestigationTurnsCost.Value));
            Assert.That(_investigatorsProvider.First.Hints.Value, Is.EqualTo(1));
        }
    }
}
