using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionRestorePhaseTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator RestorePhaseTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;

            yield return PlayAllInvestigators(withResources: true);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator.HandZone.Cards, investigator.DeckZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.HandZone.Cards, investigator2.DeckZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator.FullDeck.Take(9), investigator.HandZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.FullDeck.Take(9), investigator2.HandZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(investigator.CurrentTurns, 2).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(investigator2.CurrentTurns, 0).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(investigator2.MaxTurns, 4).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator.FullDeck.TakeLast(3), investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.FullDeck.TakeLast(3), investigator2.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(investigator.FullDeck.TakeLast(3).Select(card => card.Exausted), true).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(investigator2.FullDeck.TakeLast(3).Select(card => card.Exausted), true).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new RestorePhaseGameAction());
            yield return ClickedIn(investigator.HandZone.Cards[1]);
            yield return ClickedIn(investigator.HandZone.Cards[2]);
            yield return ClickedMainButton();
            yield return ClickedIn(investigator2.HandZone.Cards[1]);
            yield return ClickedIn(investigator2.HandZone.Cards[2]);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.HandZone.Cards.Count, Is.EqualTo(8));
            Assert.That(investigator2.HandZone.Cards.Count, Is.EqualTo(8));
            Assert.That(investigator.Resources.Value, Is.EqualTo(6));
            Assert.That(investigator2.Resources.Value, Is.EqualTo(6));
            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(3));
            Assert.That(investigator2.CurrentTurns.Value, Is.EqualTo(4));
            Assert.That(investigator.FullDeck.ElementAt(10).Exausted.IsActive, Is.False);
            Assert.That(investigator2.FullDeck.ElementAt(10).Exausted.IsActive, Is.False);
        }

        //protected override TestsType TestsType => TestsType.Debug;
        [UnityTest]
        public IEnumerator CheckMaxHandSizeUndo()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            yield return PlayAllInvestigators(withResources: true, withAvatar: false);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator.DeckZone.Cards.Take(5), investigator.HandZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.DeckZone.Cards.Take(5), investigator2.HandZone).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new RestorePhaseGameAction());
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return ClickedMainButton();
            yield return ClickedIn(investigator2.HandZone.Cards.First());
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            Assume.That(investigator.HandZone.Cards.Count, Is.EqualTo(9));
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return ClickedMainButton();
            yield return ClickedIn(investigator2.HandZone.Cards.First());
            yield return ClickedIn(investigator2.HandZone.Cards.First());
            yield return ClickedMainButton();

            yield return gameActionTask.AsCoroutine();
            Assert.That(investigator.HandZone.Cards.Count, Is.EqualTo(8));
            Assert.That(investigator2.HandZone.Cards.Count, Is.EqualTo(8));
        }
    }
}
