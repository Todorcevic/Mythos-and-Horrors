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
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator UndoMoveMulticardsTest()
        {
            _prepareGameUseCase.Execute();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            MoveCardsGameAction moveCardsGA = new(_investigatorsProvider.First.FullDeck, _investigatorsProvider.First.DiscardZone);
            yield return _gameActionsProvider.Create(moveCardsGA).AsCoroutine();

            yield return moveCardsGA.Undo().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.First.FullDeck.All(card => card.CurrentZone == _investigatorsProvider.First.FullDeck), Is.True);
        }

        [UnityTest]
        public IEnumerator UndoOneCardTest()
        {
            _prepareGameUseCase.Execute();
            Card cardTomove = _investigatorsProvider.First.FullDeck.First();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardTomove, _investigatorsProvider.First.AidZone)).AsCoroutine();
            MoveCardsGameAction moveCardsGA = new(cardTomove, _investigatorsProvider.First.DiscardZone);
            yield return _gameActionsProvider.Create(moveCardsGA).AsCoroutine();

            yield return moveCardsGA.Undo().AsCoroutine();

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

            yield return allInvestigatorsDrawCardAndResource.Undo().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.Resources.Value == 0), Is.True);
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count() == 0), Is.True);
        }

    }
}
