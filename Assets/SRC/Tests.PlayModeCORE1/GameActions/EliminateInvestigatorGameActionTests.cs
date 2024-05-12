using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class EliminateInvestigatorGameActionTests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator ElimiateInvestigatorTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardAdversity adversity = _cardsProvider.GetCard<Card01162>();
            CardCreature ghoulSecuaz = _preparationSceneCORE1.SceneCORE1.GhoulSecuaz;
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ghoulSecuaz, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(adversity, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, _preparationSceneCORE1.SceneCORE1.Attic.Hints, 2)).AsCoroutine();
            yield return _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, ghoulSecuaz, amountFear: 8)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(investigator.Defeated.IsActive, Is.True);
            Assert.That(adversity.CurrentZone, Is.EqualTo(_preparationSceneCORE1.SceneCORE1.DangerDiscardZone));
            Assert.That(ghoulSecuaz.CurrentPlace, Is.EqualTo(_preparationSceneCORE1.SceneCORE1.Study));
            Assert.That(_preparationSceneCORE1.SceneCORE1.Study.Hints.Value, Is.EqualTo(10));
        }
    }
}
