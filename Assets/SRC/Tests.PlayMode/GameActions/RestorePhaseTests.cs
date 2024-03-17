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
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RestorePhaseTest()
        {
            _prepareGameUseCase.Execute();
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.FullDeck.Take(9).ToList(), _investigatorsProvider.First.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck.Take(3).ToList(), _investigatorsProvider.Second.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.FullDeck.Skip(20).Take(5), _investigatorsProvider.First.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck.Skip(20).Take(5), _investigatorsProvider.Second.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, 2)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.Second.CurrentTurns, 0)).AsCoroutine();
            _investigatorsProvider.Second.MaxTurns.UpdateValue(4);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.FullDeck.ElementAt(10), _investigatorsProvider.First.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.FullDeck.ElementAt(12), _investigatorsProvider.First.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.FullDeck.ElementAt(14), _investigatorsProvider.First.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck.ElementAt(11), _investigatorsProvider.Second.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck.ElementAt(13), _investigatorsProvider.Second.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck.ElementAt(15), _investigatorsProvider.Second.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new ExhaustCardsGameAction(_investigatorsProvider.Second.FullDeck.ElementAt(11))).AsCoroutine();
            yield return _gameActionsProvider.Create(new ExhaustCardsGameAction(_investigatorsProvider.First.FullDeck.ElementAt(10))).AsCoroutine();
            yield return _gameActionsProvider.Create(new ExhaustCardsGameAction(_investigatorsProvider.First.FullDeck.ElementAt(12))).AsCoroutine();
            yield return _gameActionsProvider.Create(new ExhaustCardsGameAction(_investigatorsProvider.Second.FullDeck.ElementAt(13))).AsCoroutine();
            yield return _gameActionsProvider.Create(new ExhaustCardsGameAction(_investigatorsProvider.First.FullDeck.ElementAt(14))).AsCoroutine();
            yield return _gameActionsProvider.Create(new ExhaustCardsGameAction(_investigatorsProvider.Second.FullDeck.ElementAt(15))).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(_investigatorsProvider.First.HandZone.Cards[1]).AsTask();
            if (!DEBUG_MODE) WaitToClick(_investigatorsProvider.First.HandZone.Cards[2]).AsTask();
            yield return _gameActionsProvider.Create(new RestorePhaseGameAction()).AsCoroutine();

            yield return new WaitForSeconds(1);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.First.HandZone.Cards.Count, Is.EqualTo(8));
            Assert.That(_investigatorsProvider.Second.HandZone.Cards.Count, Is.EqualTo(4));
            Assert.That(_investigatorsProvider.First.Resources.Value, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.Second.Resources.Value, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.First.CurrentTurns.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Second.CurrentTurns.Value, Is.EqualTo(4));
            Assert.That(_investigatorsProvider.First.FullDeck.ElementAt(10).Exausted.IsActive, Is.False);
            Assert.That(_investigatorsProvider.Second.FullDeck.ElementAt(10).Exausted.IsActive, Is.False);
        }
    }
}
