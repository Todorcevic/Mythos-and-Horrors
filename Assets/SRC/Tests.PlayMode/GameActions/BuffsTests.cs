using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class BuffsTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator BuffTest()
        {
            _prepareGameUseCase.Execute();
            Card cardWithBuff = _cardsProvider.GetCard("01530");
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.InvestigatorCard, _investigatorsProvider.Leader.InvestigatorZone)).AsCoroutine();

            yield return _gameActionFactory.Create(new MoveCardsGameAction(cardWithBuff, _investigatorsProvider.Leader.AidZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.Leader.Intelligence.Value, Is.EqualTo(_investigatorsProvider.Leader.InvestigatorCard.Info.Intelligence + 1));
        }
    }
}
