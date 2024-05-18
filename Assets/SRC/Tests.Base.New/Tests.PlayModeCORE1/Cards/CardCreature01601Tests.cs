using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CardCreature01601Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator Parlay()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator, withResources: true);
            CardCreature creature = _cardsProvider.GetCard<Card01601>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, SceneCORE1.Study.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(creature, 2);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(creature.CurrentZone, Is.EqualTo(_investigatorsProvider.Third.DiscardZone));
            Assert.That(investigator.Resources.Value, Is.EqualTo(1));
        }
    }
}
