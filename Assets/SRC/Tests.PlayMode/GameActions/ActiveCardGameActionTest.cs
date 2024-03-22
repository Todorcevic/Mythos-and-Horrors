using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ActiveCardGameActionTest : TestBase
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator ActivateCardGameActionTest()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);
            CardSupply aidCard = _cardsProvider.GetCard<CardSupply>("01535");
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.Second.Health, 2)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.Second.CurrentTurns, _investigatorsProvider.Second.MaxTurns.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(aidCard, _investigatorsProvider.Second.AidZone)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(aidCard).AsTask();
            if (!DEBUG_MODE) WaitToClick(_investigatorsProvider.Second.AvatarCard).AsTask();
            yield return _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.Second)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.Second.Health.Value, Is.EqualTo(3));
        }
    }
}
