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
    public class ExhaustGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator ExhaustTest()
        {
            _prepareGameUseCase.Execute();
            Card cardToExhaust = _investigatorsProvider.Leader.FullDeck.First();

            yield return _gameActionFactory.Create(new MoveCardsGameAction(cardToExhaust, _investigatorsProvider.Leader.AidZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new ExhaustCardGameAction(cardToExhaust)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(cardToExhaust.Exausted.IsActive);
        }
    }
}
