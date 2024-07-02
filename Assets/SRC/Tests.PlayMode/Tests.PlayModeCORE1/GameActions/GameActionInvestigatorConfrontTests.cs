using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionInvestigatorConfrontTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator InvestigatorConfrontTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            yield return StartingScene();
            yield return _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(creature.Exausted, true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.CurrentPlace.OwnZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(creature, 1);
            yield return ClickedMainButton();

            yield return gameActionTask.AsCoroutine();
            Assert.That(creature.CurrentZone, Is.EqualTo(investigator.DangerZone));
        }
    }
}
