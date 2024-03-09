using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ConfrontEffectTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator ConfrontTest()
        {
            _prepareGameUseCase.Execute();
            CardCreature creature = _cardsProvider.AllCards.OfType<CardCreature>().First();
            CardPlace place = _cardsProvider.AllCards.OfType<CardPlace>().First();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.Turns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(creature, place.OwnZone)).AsCoroutine();
            if (!DEBUG_MODE) WaitToHistoryPanelClick().AsTask();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.AvatarCard, place.OwnZone)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(creature).AsTask();
            if (!DEBUG_MODE) WaitToCloneClick(creature.ConfrontEffect).AsTask();
            yield return _gameActionFactory.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.Leader)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(creature.CurrentZone, Is.EqualTo(_investigatorsProvider.Leader.DangerZone));
        }
    }
}
