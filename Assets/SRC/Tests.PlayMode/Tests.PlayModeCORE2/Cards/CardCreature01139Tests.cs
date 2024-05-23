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
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, investigator.CurrentPlace.Hints, 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cultist, investigator.DangerZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(cultist, 2);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cultist.CurrentZone, Is.EqualTo(SceneCORE2.VictoryZone));
            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
        }
    }
}
