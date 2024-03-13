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
    public class RestorePhaseTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RestorePhaseTest()
        {
            _prepareGameUseCase.Execute();

            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.FullDeck.Take(9).ToList(), _investigatorsProvider.Leader.HandZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck.Take(3).ToList(), _investigatorsProvider.Second.HandZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.FullDeck.GetRange(20, 5).ToList(), _investigatorsProvider.Leader.DeckZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck.GetRange(20, 5).ToList(), _investigatorsProvider.Second.DeckZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.Turns, 2)).AsCoroutine();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Second.Turns, 0)).AsCoroutine();
            _investigatorsProvider.Second.Turns.ChangeMaxValue(4);
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.FullDeck[10], _investigatorsProvider.Leader.AidZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.FullDeck[12], _investigatorsProvider.Leader.AidZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.FullDeck[14], _investigatorsProvider.Leader.AidZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck[11], _investigatorsProvider.Second.AidZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck[13], _investigatorsProvider.Second.AidZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck[15], _investigatorsProvider.Second.AidZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new ExhaustCardsGameAction(_investigatorsProvider.Second.FullDeck[11])).AsCoroutine();
            yield return _gameActionFactory.Create(new ExhaustCardsGameAction(_investigatorsProvider.Leader.FullDeck[10])).AsCoroutine();
            yield return _gameActionFactory.Create(new ExhaustCardsGameAction(_investigatorsProvider.Leader.FullDeck[12])).AsCoroutine();
            yield return _gameActionFactory.Create(new ExhaustCardsGameAction(_investigatorsProvider.Second.FullDeck[13])).AsCoroutine();
            yield return _gameActionFactory.Create(new ExhaustCardsGameAction(_investigatorsProvider.Leader.FullDeck[14])).AsCoroutine();
            yield return _gameActionFactory.Create(new ExhaustCardsGameAction(_investigatorsProvider.Second.FullDeck[15])).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(_investigatorsProvider.Leader.HandZone.Cards[1]).AsTask();
            if (!DEBUG_MODE) WaitToClick(_investigatorsProvider.Leader.HandZone.Cards[2]).AsTask();
            yield return _gameActionFactory.Create(new RestorePhaseGameAction()).AsCoroutine();

            yield return new WaitForSeconds(1);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.Leader.HandZone.Cards.Count, Is.EqualTo(8));
            Assert.That(_investigatorsProvider.Second.HandZone.Cards.Count, Is.EqualTo(4));
            Assert.That(_investigatorsProvider.Leader.Resources.Value, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.Second.Resources.Value, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.Leader.Turns.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Second.Turns.Value, Is.EqualTo(4));
            Assert.That(_investigatorsProvider.Leader.FullDeck[10].Exausted.IsActive, Is.False);
            Assert.That(_investigatorsProvider.Second.FullDeck[10].Exausted.IsActive, Is.False);
        }
    }
}
