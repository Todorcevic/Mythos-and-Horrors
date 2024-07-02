using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCreature01138Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Parley()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            CardCreature cultist = SceneCORE2.Herman;
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cultist, investigator.DangerZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(cultist, 2);
            yield return ClickedIn(investigator.DiscardableCardsInHand.First());
            yield return ClickedIn(investigator.DiscardableCardsInHand.First());
            yield return ClickedIn(investigator.DiscardableCardsInHand.First());
            yield return ClickedIn(investigator.DiscardableCardsInHand.First());
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cultist.CurrentZone, Is.EqualTo(SceneCORE2.VictoryZone));
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(0));
        }
    }
}
