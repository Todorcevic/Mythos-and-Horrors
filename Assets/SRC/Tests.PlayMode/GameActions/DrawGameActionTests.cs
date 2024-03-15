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
        [Inject] private readonly GameActionProvider _gameActionFactory;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator DrawTest()
        {
            _prepareGameUseCase.Execute();

            Card cardToDraw = _investigatorsProvider.Leader.FullDeck.First();

            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(cardToDraw, _investigatorsProvider.Leader.DeckZone)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(cardToDraw).AsTask();
            yield return _gameActionFactory.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.Leader)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.Leader.HandZone.Cards.First(), Is.EqualTo(cardToDraw));
        }
    }
}
