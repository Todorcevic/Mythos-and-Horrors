using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class GameActionParentTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Parent_GameAction_Test()
        {
            _prepareGameUseCase.Execute();

            Investigator investigator = _investigatorsProvider.First;
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.FullDeck.ElementAt(0), investigator.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.FullDeck.ElementAt(1), investigator.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.FullDeck.ElementAt(2), investigator.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.FullDeck.ElementAt(3), investigator.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.FullDeck.ElementAt(4), investigator.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.FullDeck.ElementAt(5), investigator.DeckZone)).AsCoroutine();

            InitialDrawGameAction initialDrawGameAction = new(investigator);
            yield return _gameActionsProvider.Create(initialDrawGameAction).AsCoroutine();

            Assert.That(_gameActionsProvider.AllGameActionsCreated[7].GetType(), Is.EqualTo(typeof(DrawAidGameAction)));
            Assert.That(_gameActionsProvider.AllGameActionsCreated[7].Parent, Is.EqualTo(initialDrawGameAction));
        }
    }
}
