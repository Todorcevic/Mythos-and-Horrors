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
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cultist, investigator.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(cultist, investigator.InvestigatorCard, amountDamage: 2).Start().AsCoroutine();

            yield return _gameActionsProvider.Create<CreatureAttackGameAction>().SetWith(cultist, _investigatorsProvider.First).Start().AsCoroutine();
            Assert.That(cultist.HealthLeft, Is.EqualTo(3));
            yield return _gameActionsProvider.Create<CreatureAttackGameAction>().SetWith(cultist, _investigatorsProvider.First).Start().AsCoroutine();
            Assert.That(cultist.HealthLeft, Is.EqualTo(4));
            yield return _gameActionsProvider.Create<CreatureAttackGameAction>().SetWith(cultist, _investigatorsProvider.First).Start().AsCoroutine();
            Assert.That(cultist.HealthLeft, Is.EqualTo(4));
        }
    }
}
