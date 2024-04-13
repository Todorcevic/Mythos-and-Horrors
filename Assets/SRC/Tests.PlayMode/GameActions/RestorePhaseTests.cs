using MythosAndHorrors.GameRules;
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
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RestorePhaseTest()
        {
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.FullDeck.Take(9).ToList(), _investigatorsProvider.First.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck.Take(3).ToList(), _investigatorsProvider.Second.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, 2)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.Second.CurrentTurns, 0)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.Second.MaxTurns, 4)).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.FullDeck.TakeLast(3), _investigatorsProvider.First.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck.TakeLast(3), _investigatorsProvider.Second.AidZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new UpdateStatesGameAction(_investigatorsProvider.First.FullDeck.TakeLast(3).Select(card => card.Exausted), true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatesGameAction(_investigatorsProvider.Second.FullDeck.TakeLast(3).Select(card => card.Exausted), true)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(_investigatorsProvider.First.HandZone.Cards.ElementAt(1)).AsTask();
            if (!DEBUG_MODE) WaitToClick(_investigatorsProvider.First.HandZone.Cards.ElementAt(2)).AsTask();
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
