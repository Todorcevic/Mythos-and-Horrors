using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class UndoGameActionTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator UndoMoveMulticardsTest()
        {
            yield return _preparationSceneCORE1.PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.HandZone.Cards, _investigatorsProvider.First.DeckZone)).AsCoroutine();
            MoveCardsGameAction moveCardsGameAction = new(_investigatorsProvider.First.FullDeck, _investigatorsProvider.First.DiscardZone);
            yield return _gameActionsProvider.Create(moveCardsGameAction).AsCoroutine();

            yield return _gameActionsProvider.UndoLast().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.First.FullDeck.All(card => card.CurrentZone == _investigatorsProvider.First.DeckZone), Is.True);
        }

        [UnityTest]
        public IEnumerator UndoOneCardTest()
        {
            Card cardTomove = _investigatorsProvider.First.FullDeck.First();
            yield return _preparationSceneCORE1.PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardTomove, _investigatorsProvider.First.AidZone)).AsCoroutine();
            MoveCardsGameAction moveCardGameAction = new(cardTomove, _investigatorsProvider.First.DiscardZone);
            yield return _gameActionsProvider.Create(moveCardGameAction).AsCoroutine();

            yield return _gameActionsProvider.UndoLast().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardTomove.CurrentZone, Is.EqualTo(_investigatorsProvider.First.AidZone));
        }

        [UnityTest]
        public IEnumerator UndoAllInvestigatorDrawTest()
        {
            yield return _preparationSceneCORE1.PlayAllInvestigators();
            AllInvestigatorsDrawCardAndResource allInvestigatorsDrawCardAndResource = new();
            yield return _gameActionsProvider.Create(allInvestigatorsDrawCardAndResource).AsCoroutine();

            yield return _gameActionsProvider.Rewind().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.Resources.Value == 0), Is.True);
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count == 0), Is.True);
        }

        [UnityTest]
        public IEnumerator UndoRestorePhaseGameActionTest()
        {
            yield return _preparationSceneCORE1.PlayAllInvestigators();
            RestorePhaseGameAction restorePhaseGameAction = new();
            yield return _gameActionsProvider.Create(restorePhaseGameAction).AsCoroutine();

            yield return _gameActionsProvider.Rewind().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.Resources.Value == 0), Is.True);
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count == 0), Is.True);
        }

        [UnityTest]
        public IEnumerator UndoTursStatTest()
        {
            yield return _preparationSceneCORE1.PlayThisInvestigator(_investigatorsProvider.First);
            Task gameActionTask = _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());

            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.CardAidToDraw);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.CardAidToDraw);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.CardAidToDraw);

            yield return gameActionTask.AsCoroutine();
            yield return _gameActionsProvider.Rewind().AsCoroutine();
            Assert.That(_investigatorsProvider.GetInvestigatorsCanStartTurn.Count(), Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator FullUndoTest()
        {
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new RevealGameAction(_preparationSceneCORE1.SceneCORE1.Attic)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, _preparationSceneCORE1.SceneCORE1.Hallway)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());

            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.AvatarCard);
            if (!DEBUG_MODE) yield return WaitToClick(_preparationSceneCORE1.SceneCORE1.Attic);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.InvestigatorCard);
            Assert.That(_investigatorsProvider.First.Sanity.Value, Is.EqualTo(4));

            if (!DEBUG_MODE) yield return WaitToUndoClick();
            if (!DEBUG_MODE) yield return WaitToUndoClick();
            Assert.That(_investigatorsProvider.First.Sanity.Value, Is.EqualTo(5));
            Assert.That(_investigatorsProvider.First.CurrentPlace, Is.EqualTo(_preparationSceneCORE1.SceneCORE1.Hallway));

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.AvatarCard);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Third.AvatarCard);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Fourth.AvatarCard);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return gameActionTask.AsCoroutine();
            yield return _gameActionsProvider.Rewind().AsCoroutine();
            Assert.That(_investigatorsProvider.GetInvestigatorsCanStartTurn.Count(), Is.EqualTo(0));
        }
    }
}
