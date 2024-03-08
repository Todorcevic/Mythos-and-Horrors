using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ActiveCardGameActionTest : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator ActivateCardGameActionTest()
        {
            _prepareGameUseCase.Execute();
            CardSupply aidCard = _cardsProvider.GetCard<CardSupply>("01535");

            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Second.Health, 2)).AsCoroutine();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Second.Turns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(aidCard, _investigatorsProvider.Second.AidZone)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(aidCard).AsTask();
            if (!DEBUG_MODE) WaitToClick(_investigatorsProvider.Second.AvatarCard).AsTask();
            yield return _gameActionFactory.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.Second)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.Second.Health.Value, Is.EqualTo(3));
        }
    }
}
