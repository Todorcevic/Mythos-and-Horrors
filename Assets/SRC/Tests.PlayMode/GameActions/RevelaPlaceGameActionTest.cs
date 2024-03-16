using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class RevelaPlaceGameActionTest : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardViewsManager _cardViewsManager;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RevealPlaceTest()
        {
            _prepareGameUseCase.Execute();
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01111");
            yield return _gameActionFactory.Create(new MoveCardsGameAction(place, _chapterProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
            if (!DEBUG_MODE) WaitToHistoryPanelClick().AsTask();

            yield return _gameActionFactory.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, place)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That((_cardViewsManager.GetCardView(place) as PlaceCardView).GetPrivateMember<StatView>("_hints").isActiveAndEnabled);
        }
    }
}
