using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class GameActionUndoTests : TestCORE1Preparation
    {

        [UnityTest]
        public IEnumerator UndoMoveMulticardsTest()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.HandZone.Cards, _investigatorsProvider.First.DeckZone)).AsCoroutine();
            MoveCardsGameAction moveCardsGameAction = new(_investigatorsProvider.First.FullDeck, _investigatorsProvider.First.DiscardZone);
            yield return _gameActionsProvider.Create(moveCardsGameAction).AsCoroutine();

            yield return _gameActionsProvider.UndoLast().AsCoroutine();

            Assert.That(_investigatorsProvider.First.FullDeck.All(card => card.CurrentZone == _investigatorsProvider.First.DeckZone), Is.True);
        }

        [UnityTest]
        public IEnumerator UndoOneCardTest()
        {
            Card cardTomove = _investigatorsProvider.First.FullDeck.First();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardTomove, _investigatorsProvider.First.AidZone)).AsCoroutine();
            MoveCardsGameAction moveCardGameAction = new(cardTomove, _investigatorsProvider.First.DiscardZone);
            yield return _gameActionsProvider.Create(moveCardGameAction).AsCoroutine();

            yield return _gameActionsProvider.UndoLast().AsCoroutine();

            Assert.That(cardTomove.CurrentZone, Is.EqualTo(_investigatorsProvider.First.AidZone));
        }

        [UnityTest]
        public IEnumerator UndoAllInvestigatorDrawTest()
        {
            yield return PlayAllInvestigators();
            AllInvestigatorsDrawCardAndResource allInvestigatorsDrawCardAndResource = new();
            yield return _gameActionsProvider.Create(allInvestigatorsDrawCardAndResource).AsCoroutine();

            yield return _gameActionsProvider.Rewind().AsCoroutine();

            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.Resources.Value == 0), Is.True);
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count == 0), Is.True);
        }

        [UnityTest]
        public IEnumerator UndoRestorePhaseGameActionTest()
        {
            yield return PlayAllInvestigators();
            RestorePhaseGameAction restorePhaseGameAction = new();
            yield return _gameActionsProvider.Create(restorePhaseGameAction).AsCoroutine();

            yield return _gameActionsProvider.Rewind().AsCoroutine();

            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.Resources.Value == 0), Is.True);
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count == 0), Is.True);
        }

        [UnityTest]
        public IEnumerator UndoTursStatTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlayThisInvestigator(investigator);
            Task gameActionTask = _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());
            yield return ClickedIn(investigator.CardAidToDraw);
            yield return ClickedIn(investigator.CardAidToDraw);
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return ClickedIn(investigator.CardAidToDraw);
            yield return ClickedIn(investigator.InvestigatorCard);

            yield return gameActionTask.AsCoroutine();
            yield return _gameActionsProvider.Rewind().AsCoroutine();

            Assert.That(_investigatorsProvider.GetInvestigatorsCanStartTurn.Count(), Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator FullUndoTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new RevealGameAction(SceneCORE1.Attic)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, SceneCORE1.Hallway)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());

            yield return ClickedIn(investigator.AvatarCard);
            yield return ClickedIn(SceneCORE1.Attic);
            yield return ClickedIn(investigator.InvestigatorCard);

            Assume.That(investigator.FearRecived, Is.EqualTo(1));

            yield return ClickedUndoButton();
            yield return ClickedUndoButton();

            Assume.That(investigator.Sanity.Value, Is.EqualTo(5));
            Assume.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE1.Hallway));

            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Third.AvatarCard);
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Fourth.AvatarCard);
            yield return ClickedMainButton();

            yield return gameActionTask.AsCoroutine();
            yield return _gameActionsProvider.Rewind().AsCoroutine();

            Assert.That(_investigatorsProvider.GetInvestigatorsCanStartTurn.Count(), Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator FullUndoTest2()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();

            Task gameActionTask = _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());
            yield return ClickedIn(investigator.AvatarCard);
            yield return ClickedTokenButton();
            yield return ClickedTokenButton();
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedTokenButton();
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            yield return ClickedTokenButton();
            Assume.That(investigator.CurrentTurns.Value, Is.EqualTo(0));
            yield return ClickedIn(_investigatorsProvider.Third.AvatarCard);
            yield return ClickedTokenButton();
            Assume.That(_investigatorsProvider.Second.CurrentTurns.Value, Is.EqualTo(3));
            Assume.That(_investigatorsProvider.Third.CurrentTurns.Value, Is.EqualTo(2));
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Fourth.AvatarCard);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.GetInvestigatorsCanStartTurn.Count(), Is.EqualTo(0));
        }
    }
}
