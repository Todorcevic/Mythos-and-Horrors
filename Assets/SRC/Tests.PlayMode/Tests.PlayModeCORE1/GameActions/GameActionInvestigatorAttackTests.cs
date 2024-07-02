using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionInvestigatorAttackTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator InvestigatorAttackInDangerZoneTest()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, _investigatorsProvider.First.DangerZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.First).Execute();
            yield return ClickedClone(creature, 0);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(creature.HealthLeft, Is.EqualTo(creature.Info.Health - 1));
        }

        [UnityTest]
        public IEnumerator InvestigatorAttackInPlaceZoneTest()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, _investigatorsProvider.First.CurrentPlace.OwnZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.First).Execute();
            yield return ClickedClone(creature, 0);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(creature.HealthLeft, Is.EqualTo(creature.Info.Health - 1));
        }

        [UnityTest]
        public IEnumerator InvestigatorAttackToCreatureConfrontedWithOtherAndFail()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, _investigatorsProvider.First.DangerZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.Second).Execute();
            yield return ClickedClone(creature, 0);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(creature.HealthLeft, Is.EqualTo(creature.Info.Health));
            Assert.That(_investigatorsProvider.First.DamageRecived.Value, Is.EqualTo(1));
        }
    }
}
