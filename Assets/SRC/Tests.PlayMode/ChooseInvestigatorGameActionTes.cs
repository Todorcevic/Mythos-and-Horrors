using ModestTree;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    public class ChooseInvestigatorGameActionTes : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;

        protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RevealPlaceTest()
        {
            _prepareGameUseCase.Execute();
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01111");
            yield return _gameActionFactory.Create(new MoveCardsGameAction(place, _chapterProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
            if (!DEBUG_MODE) WaitToClickHistoryPanel().AsTask();
            yield return _gameActionFactory.Create(new MoveInvestigatorGameAction(_investigatorsProvider.AllInvestigators, place)).AsCoroutine();

            _gameActionFactory.Create(new ChooseInvestigatorGameAction()).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_zoneViewsManager.SelectorZone.GetComponentsInChildren<CardView>().Length == 4);
        }
    }
}
