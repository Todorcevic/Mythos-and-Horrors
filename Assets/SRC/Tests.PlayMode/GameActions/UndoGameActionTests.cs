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
    public class UndoGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator UndoMoveMulticardsTest()
        {
            _prepareGameUseCase.Execute();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            MoveCardsGameAction moveCardsGameAction = new(_investigatorsProvider.First.FullDeck, _investigatorsProvider.First.DiscardZone);
            yield return _gameActionsProvider.Create(moveCardsGameAction).AsCoroutine();

            yield return moveCardsGameAction.Undo().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.First.FullDeck.All(card => card.CurrentZone == _investigatorsProvider.First.DeckZone), Is.True);
        }

        [UnityTest]
        public IEnumerator UndoOneCardTest()
        {
            _prepareGameUseCase.Execute();
            Card cardTomove = _investigatorsProvider.First.FullDeck.First();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardTomove, _investigatorsProvider.First.AidZone)).AsCoroutine();
            MoveCardsGameAction moveCardGameAction = new(cardTomove, _investigatorsProvider.First.DiscardZone);
            yield return _gameActionsProvider.Create(moveCardGameAction).AsCoroutine();

            yield return moveCardGameAction.Undo().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardTomove.CurrentZone, Is.EqualTo(_investigatorsProvider.First.AidZone));
        }

        [UnityTest]
        public IEnumerator UndoAllInvestigatorDrawTest()
        {
            _prepareGameUseCase.Execute();
            yield return PlayAllInvestigators();

            AllInvestigatorsDrawCardAndResource allInvestigatorsDrawCardAndResource = new();
            yield return _gameActionsProvider.Create(allInvestigatorsDrawCardAndResource).AsCoroutine();

            yield return _gameActionsProvider.UndoRewind().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.Resources.Value == 0), Is.True);
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count() == 0), Is.True);
        }

        [UnityTest]
        public IEnumerator UndoRestorePhaseGameActionTest()
        {
            _prepareGameUseCase.Execute();
            yield return PlayAllInvestigators();
            RestorePhaseGameAction restorePhaseGameAction = new();
            yield return _gameActionsProvider.Create(restorePhaseGameAction).AsCoroutine();

            yield return _gameActionsProvider.UndoRewind().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.Resources.Value == 0), Is.True);
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count() == 0), Is.True);
        }

        [UnityTest]
        public IEnumerator UndoTursStatTest()
        {
            _prepareGameUseCase.Execute();
            yield return PlayThisInvestigator(_investigatorsProvider.First);

            yield return _gameActionsProvider.Create(new ResetAllInvestigatorsTurnsGameAction()).AsCoroutine();

            if (DEBUG_MODE) yield return _gameActionsProvider.Create(new InvestigatorsPhaseGameAction()).AsCoroutine();
            else _ = _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());

            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Leader.CardAidToDraw);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Leader.CardAidToDraw);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Leader.CardAidToDraw);

            yield return _gameActionsProvider.UndoRewind().AsCoroutine();


            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Count(), Is.EqualTo(0));
        }
    }
}
