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
    public class EludeEffectTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator EludeEffectTest()
        {
            _prepareGameUseCase.Execute();
            CardCreature creature = _cardsProvider.AllCards.OfType<CardCreature>().First();
            CardPlace place = _cardsProvider.AllCards.OfType<CardPlace>().First();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.Turns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            if (!DEBUG_MODE) WaitToHistoryPanelClick().AsTask();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.AvatarCard, place.OwnZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(creature, _investigatorsProvider.Leader.DangerZone)).AsCoroutine();

            OneInvestigatorTurnGameAction oiGA = new(_investigatorsProvider.Leader);
            _ = _gameActionFactory.Create(oiGA);
            if (!DEBUG_MODE) yield return WaitToClick(creature);
            if (!DEBUG_MODE) yield return WaitToCloneClick(oiGA.InvestigatorEludeEffects.Find(effect => effect.CardAffected == creature));

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(creature.Exausted.IsActive);
            Assert.That(creature.CurrentPlace, Is.EqualTo(place));
        }
    }
}