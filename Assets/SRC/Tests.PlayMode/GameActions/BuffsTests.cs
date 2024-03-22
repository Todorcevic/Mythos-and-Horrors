using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{

    public class BuffsTests : TestBase
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator BuffTest()
        {
            Card cardWithBuff = _cardsProvider.GetCard("01530");
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardWithBuff, _investigatorsProvider.First.AidZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.First.Intelligence.Value, Is.EqualTo(_investigatorsProvider.First.InvestigatorCard.Info.Intelligence + 1));
            Assert.That(_investigatorsProvider.Second.Intelligence.Value, Is.EqualTo(_investigatorsProvider.Second.InvestigatorCard.Info.Intelligence + 1));

        }
    }
}
