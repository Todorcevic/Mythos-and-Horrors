using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class GameActionInvestigatorConfrontTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator InvestigatorConfrontTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new UpdateStatesGameAction(creature.Exausted, true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, investigator.CurrentPlace.OwnZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(creature, 1);
            yield return ClickedMainButton();

            yield return gameActionTask.AsCoroutine();
            Assert.That(creature.CurrentZone, Is.EqualTo(investigator.DangerZone));
        }
    }
}
