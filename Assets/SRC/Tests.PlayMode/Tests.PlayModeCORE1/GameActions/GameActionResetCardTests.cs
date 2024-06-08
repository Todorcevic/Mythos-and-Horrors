using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionResetCardTests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ResetCard()
        {
            Investigator investigator = _investigatorsProvider.Second;
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatesGameAction(creature.Exausted, true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new HarmToCardGameAction(creature, investigator.InvestigatorCard, amountDamage: 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, SceneCORE1.DangerDeckZone, isFaceDown: true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DrawDangerGameAction(investigator)).AsCoroutine();

            Assert.That(creature.Exausted.IsActive, Is.False);
            Assert.That(creature.DamageRecived.Value, Is.EqualTo(0));
        }
    }
}