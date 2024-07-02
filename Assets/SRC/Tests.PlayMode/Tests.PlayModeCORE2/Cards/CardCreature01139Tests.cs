using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCreature01139Tests : TestCORE2Preparation
    {
        [UnityTest]
        public IEnumerator Parley()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            CardCreature cultist = SceneCORE2.Peter;
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(investigator, investigator.CurrentPlace.Hints, 3).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cultist, investigator.DangerZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(cultist, 2);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cultist.CurrentZone, Is.EqualTo(SceneCORE2.VictoryZone));
            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
        }
    }
}
