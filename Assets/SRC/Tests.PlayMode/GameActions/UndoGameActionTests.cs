using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
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

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator UndoTest()
        {
            //_prepareGameUseCase.Execute();
            //yield return PlayThisInvestigator(_investigatorsProvider.First);


            //yield return _gameActionsProvider.Create(new MoveCardsGameAction(card, _investigatorsProvider.First.AidZone)).AsCoroutine();




            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            //Assert.That(_investigatorsProvider.First.AidZone.Cards, Has.No.Member(card));
        }
    }
}
