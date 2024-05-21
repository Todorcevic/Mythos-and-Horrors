using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CardCreature01140Tests : TestCORE2Preparation
    {
        [UnityTest]
        public IEnumerator Parley()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene(withResources: true);
            CardCreature cultist = SceneCORE2.Victoria;
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cultist, investigator.DangerZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(cultist, 2);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cultist.CurrentZone, Is.EqualTo(SceneCORE2.VictoryZone));
            Assert.That(investigator.Resources.Value, Is.EqualTo(0));
        }
    }
}
