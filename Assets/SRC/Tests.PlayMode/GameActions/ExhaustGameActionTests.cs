using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{

    public class ExhaustGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator ExhaustTest()
        {
            _prepareGameUseCase.Execute();
            Card cardToExhaust = _investigatorsProvider.First.FullDeck.First();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardToExhaust, _investigatorsProvider.First.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new ExhaustCardsGameAction(cardToExhaust)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(cardToExhaust.Exausted.IsActive);

            yield return _gameActionsProvider.Create(new ReadyCardGameAction(cardToExhaust)).AsCoroutine();
            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(!cardToExhaust.Exausted.IsActive);
        }
    }
}
