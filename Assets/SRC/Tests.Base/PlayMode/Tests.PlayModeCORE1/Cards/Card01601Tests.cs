using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01601Tests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PayCratureTest()
        {
            yield return _preparationSceneCORE1.PlaceAllScene();
            yield return _preparationSceneCORE1.PlayThisInvestigator(_investigatorsProvider.Third, withResources: true);
            CardCreature creature = _cardsProvider.GetCard<Card01601>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _preparationSceneCORE1.SceneCORE1.Study.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Third));
            if (!DEBUG_MODE) yield return WaitToClick(creature);
            if (!DEBUG_MODE) yield return WaitToCloneClick(2);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return taskGameAction.AsCoroutine();
            Assert.That(creature.CurrentZone, Is.EqualTo(_investigatorsProvider.Third.DiscardZone));
            Assert.That(_investigatorsProvider.Third.Resources.Value, Is.EqualTo(1));
        }
    }
}
