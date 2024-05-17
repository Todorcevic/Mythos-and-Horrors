using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class GameActionInvestigatorAttackTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator InvestigatorAttackInDangerZoneTest()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _investigatorsProvider.First.DangerZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            yield return ClickedClone(creature, 0);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(creature.Health.Value, Is.EqualTo(creature.Info.Health - 1));
        }

        [UnityTest]
        public IEnumerator InvestigatorAttackInPlaceZoneTest()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _investigatorsProvider.First.CurrentPlace.OwnZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            yield return ClickedClone(creature, 0);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(creature.Health.Value, Is.EqualTo(creature.Info.Health - 1));
        }
    }
}
