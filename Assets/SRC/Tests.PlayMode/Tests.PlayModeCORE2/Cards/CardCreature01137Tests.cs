using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCreature01137Tests : TestCORE2Preparation
    {
        [UnityTest]
        public IEnumerator HealWhenAttacked()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            CardCreature cultist = SceneCORE2.Drew;
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cultist, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new HarmToCardGameAction(cultist, investigator.InvestigatorCard, amountDamage: 2)).AsCoroutine();

            yield return _gameActionsProvider.Create(new CreatureAttackGameAction(cultist, _investigatorsProvider.First)).AsCoroutine();
            Assert.That(cultist.HealthLeft, Is.EqualTo(3));
            yield return _gameActionsProvider.Create(new CreatureAttackGameAction(cultist, _investigatorsProvider.First)).AsCoroutine();
            Assert.That(cultist.HealthLeft, Is.EqualTo(4));
            yield return _gameActionsProvider.Create(new CreatureAttackGameAction(cultist, _investigatorsProvider.First)).AsCoroutine();
            Assert.That(cultist.HealthLeft, Is.EqualTo(4));
        }
    }
}
