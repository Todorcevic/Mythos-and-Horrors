using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{

    public class GameActionEludeTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator EludeTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, investigator.DangerZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(creature, 1);
            yield return ClickedMainButton();
            yield return ClickedMainButton();

            yield return gameActionTask.AsCoroutine();

            Assert.That(creature.Exausted.IsActive, Is.True);
            Assert.That(creature.CurrentZone, Is.EqualTo(investigator.CurrentPlace.OwnZone));
        }
    }
}