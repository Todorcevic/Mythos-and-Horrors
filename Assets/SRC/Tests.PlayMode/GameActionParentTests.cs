using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class GameActionParentTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Parent_GameAction_Test()
        {
            _prepareGameUseCase.Execute();
            Investigator investigator = _investigatorsProvider.Leader;
            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator.FullDeck[0], investigator.DeckZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator.FullDeck[1], investigator.DeckZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator.FullDeck[2], investigator.DeckZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator.FullDeck[3], investigator.DeckZone)).AsCoroutine();

            InitialDrawGameAction initialDrawGameAction = new(investigator);
            yield return _gameActionFactory.Create(initialDrawGameAction).AsCoroutine();

            Assert.That(_gameActionFactory.AllGameActions[5] is DrawAidGameAction);
            Assert.That(_gameActionFactory.AllGameActions[5].Parent, Is.EqualTo(initialDrawGameAction));
        }
    }
}
