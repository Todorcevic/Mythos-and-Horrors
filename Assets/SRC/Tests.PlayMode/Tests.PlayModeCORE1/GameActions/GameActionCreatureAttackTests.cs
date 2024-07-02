using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionCreatureAttackTests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CreatureAttackTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardCreature cardCreature = _cardsProvider.GetCard<Card01119>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator.InvestigatorCard, investigator.InvestigatorZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardCreature, investigator.DangerZone).Start().AsCoroutine();

            yield return _gameActionsProvider.Create<CreatureAttackGameAction>().SetWith(cardCreature, investigator).Start().AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(2));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator CreatureAttackOtherInvestigatorTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            CardCreature cardCreature = _cardsProvider.GetCard<Card01119>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator.InvestigatorCard, investigator.InvestigatorZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.InvestigatorCard, investigator2.InvestigatorZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardCreature, investigator.DangerZone).Start().AsCoroutine();

            yield return _gameActionsProvider.Create<CreatureAttackGameAction>().SetWith(cardCreature, investigator2).Start().AsCoroutine();

            Assert.That(investigator2.DamageRecived.Value, Is.EqualTo(2));
            Assert.That(investigator2.FearRecived.Value, Is.EqualTo(1));
        }
    }
}
