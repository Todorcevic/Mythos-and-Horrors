using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class EludeEffectTests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator EludeEffectTest()
        {
            CardCreature creature = _preparationSceneCORE1.SceneCORE1.GhoulSecuaz;
            MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _investigatorsProvider.First.DangerZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(creature);
            if (!DEBUG_MODE) yield return WaitToCloneClick(1);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return gameActionTask.AsCoroutine();
            Assert.That(creature.Exausted.IsActive, Is.True);
            Assert.That(creature.CurrentZone, Is.EqualTo(_investigatorsProvider.First.CurrentPlace.OwnZone));
        }
    }
}