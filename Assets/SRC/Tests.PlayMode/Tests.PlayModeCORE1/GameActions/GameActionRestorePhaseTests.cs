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
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator RestorePhaseTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            yield return PlayThisInvestigator(investigator, withResources: true);
            yield return PlayThisInvestigator(investigator2, withResources: true);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator.HandZone.Cards, investigator.DeckZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.HandZone.Cards, investigator2.DeckZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator.FullDeck.Take(9), investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.FullDeck.Take(9), investigator2.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(investigator.CurrentActions, 2).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(investigator2.CurrentActions, 0).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(investigator2.MaxActions, 4).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator.FullDeck.TakeLast(3), investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.FullDeck.TakeLast(3), investigator2.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(investigator.FullDeck.TakeLast(3).Select(card => card.Exausted), true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(investigator2.FullDeck.TakeLast(3).Select(card => card.Exausted), true).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<RestorePhaseGameAction>().Execute();
            yield return ClickedIn(investigator.HandZone.Cards[0]);
            yield return ClickedIn(investigator.HandZone.Cards[1]);
            yield return ClickedIn(investigator2.HandZone.Cards[1]);
            yield return ClickedIn(investigator2.HandZone.Cards[2]);
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.HandZone.Cards.Count, Is.EqualTo(8));
            Assert.That(investigator2.HandZone.Cards.Count, Is.EqualTo(8));
            Assert.That(investigator.Resources.Value, Is.EqualTo(6));
            Assert.That(investigator2.Resources.Value, Is.EqualTo(6));
            Assert.That(investigator.CurrentActions.Value, Is.EqualTo(3));
            Assert.That(investigator2.CurrentActions.Value, Is.EqualTo(4));
            Assert.That(investigator.FullDeck.ElementAt(10).Exausted.IsActive, Is.False);
            Assert.That(investigator2.FullDeck.ElementAt(10).Exausted.IsActive, Is.False);
        }

        [UnityTest]
        public IEnumerator CheckMaxHandSizeUndo()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            yield return PlayAllInvestigators(withResources: true, withAvatar: false);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator.DeckZone.Cards.Take(5), investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.DeckZone.Cards.Take(5), investigator2.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<RestorePhaseGameAction>().Execute();
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return ClickedIn(investigator2.HandZone.Cards.First());
            yield return ClickedUndoButton();
            AssumeThat(investigator2.HandZone.Cards.Count == 10);
            yield return ClickedUndoButton();
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return ClickedIn(investigator2.HandZone.Cards.First());
            yield return ClickedIn(investigator2.HandZone.Cards.First());

            yield return gameActionTask.AsCoroutine();
            Assert.That(investigator.HandZone.Cards.Count, Is.EqualTo(8));
            Assert.That(investigator2.HandZone.Cards.Count, Is.EqualTo(8));
        }
    }
}
