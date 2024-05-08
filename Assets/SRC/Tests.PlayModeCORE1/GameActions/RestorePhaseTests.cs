using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class RestorePhaseTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RestorePhaseTest()
        {
            yield return _preparationScene.PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.HandZone.Cards, _investigatorsProvider.First.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.HandZone.Cards, _investigatorsProvider.Second.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.FullDeck.Take(9), _investigatorsProvider.First.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck.Take(9), _investigatorsProvider.Second.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, 2)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.Second.CurrentTurns, 0)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.Second.MaxTurns, 4)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.FullDeck.TakeLast(3), _investigatorsProvider.First.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck.TakeLast(3), _investigatorsProvider.Second.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatesGameAction(_investigatorsProvider.First.FullDeck.TakeLast(3).Select(card => card.Exausted), true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatesGameAction(_investigatorsProvider.Second.FullDeck.TakeLast(3).Select(card => card.Exausted), true)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new RestorePhaseGameAction());
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.HandZone.Cards[1]);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.HandZone.Cards[2]);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.HandZone.Cards[1]);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.HandZone.Cards[2]);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return gameActionTask.AsCoroutine();
            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(_investigatorsProvider.First.HandZone.Cards.Count, Is.EqualTo(8));
            Assert.That(_investigatorsProvider.Second.HandZone.Cards.Count, Is.EqualTo(8));
            Assert.That(_investigatorsProvider.First.Resources.Value, Is.EqualTo(6));
            Assert.That(_investigatorsProvider.Second.Resources.Value, Is.EqualTo(6));
            Assert.That(_investigatorsProvider.First.CurrentTurns.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Second.CurrentTurns.Value, Is.EqualTo(4));
            Assert.That(_investigatorsProvider.First.FullDeck.ElementAt(10).Exausted.IsActive, Is.False);
            Assert.That(_investigatorsProvider.Second.FullDeck.ElementAt(10).Exausted.IsActive, Is.False);
        }
    }
}
