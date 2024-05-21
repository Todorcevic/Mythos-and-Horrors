using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CardCreature01138Tests : TestCORE2Preparation
    {
        [UnityTest]
        public IEnumerator Parley()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            CardCreature cultist = SceneCORE2.Herman;
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cultist, investigator.DangerZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(cultist, 2);
            yield return ClickedIn(investigator.HandZone.Cards.First(card => card.CanBeDiscarded));
            yield return ClickedIn(investigator.HandZone.Cards.First(card => card.CanBeDiscarded));
            yield return ClickedIn(investigator.HandZone.Cards.First(card => card.CanBeDiscarded));
            yield return ClickedIn(investigator.HandZone.Cards.First(card => card.CanBeDiscarded));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cultist.CurrentZone, Is.EqualTo(SceneCORE2.VictoryZone));
            Assert.That(investigator.DamageRecived, Is.EqualTo(0));
        }
    }
}
