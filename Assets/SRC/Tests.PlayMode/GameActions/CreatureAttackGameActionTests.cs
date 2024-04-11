using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CreatureAttackGameActionTests : TestBase
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator CreatureAttackTest()
        {
            CardCreature cardCreature = _cardsProvider.GetCard<CardCreature>("01119");
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.InvestigatorCard, _investigatorsProvider.First.InvestigatorZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardCreature, _investigatorsProvider.First.DangerZone)).AsCoroutine();

            do
            {
                yield return _gameActionsProvider.Create(new CreatureAttackGameAction(cardCreature, _investigatorsProvider.First)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);

            yield return new WaitForSeconds(1);

            Assert.That(_investigatorsProvider.First.Health.Value, Is.EqualTo(_investigatorsProvider.First.InvestigatorCard.Info.Health - 2));
            Assert.That(_investigatorsProvider.First.Sanity.Value, Is.EqualTo(_investigatorsProvider.First.InvestigatorCard.Info.Sanity - 1));
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator CreatureAttackOtherInvestigatorTest()
        {
            CardCreature cardCreature = _cardsProvider.GetCard<CardCreature>("01119");
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.InvestigatorCard, _investigatorsProvider.First.InvestigatorZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.InvestigatorCard, _investigatorsProvider.Second.InvestigatorZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardCreature, _investigatorsProvider.First.DangerZone)).AsCoroutine();

            do
            {
                yield return _gameActionsProvider.Create(new CreatureAttackGameAction(cardCreature, _investigatorsProvider.Second)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);

            yield return new WaitForSeconds(2);

            Assert.That(_investigatorsProvider.Second.Health.Value, Is.EqualTo(_investigatorsProvider.Second.InvestigatorCard.Info.Health - 2));
            Assert.That(_investigatorsProvider.Second.Sanity.Value, Is.EqualTo(_investigatorsProvider.Second.InvestigatorCard.Info.Sanity - 1));
        }
    }
}
