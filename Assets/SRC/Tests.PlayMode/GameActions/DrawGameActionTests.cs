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
    public class DrawGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator DrawTest()
        {
            _prepareGameUseCase.Execute();

            Card cardToDraw = _investigatorsProvider.First.FullDeck.First();

            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardToDraw, _investigatorsProvider.First.DeckZone)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(cardToDraw).AsTask();
            yield return _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.First)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.First.HandZone.Cards.First(), Is.EqualTo(cardToDraw));
        }
    }
}
