using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionEliminateInvestigatorTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator EliminateInvestigatorTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardAdversity adversity = _cardsProvider.GetCard<Card01162>();
            CardCreature ghoulSecuaz = SceneCORE1.GhoulSecuaz;
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ghoulSecuaz, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(adversity, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, SceneCORE1.Attic.Hints, 2)).AsCoroutine();
            yield return _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, ghoulSecuaz, amountFear: 8)).AsCoroutine();

            Assert.That(investigator.Defeated.IsActive, Is.True);
            Assert.That(adversity.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
            Assert.That(ghoulSecuaz.CurrentPlace, Is.EqualTo(SceneCORE1.Study));
            Assert.That(SceneCORE1.Study.Hints.Value, Is.EqualTo(10));
            Assert.That(investigator.Defeated.IsActive, Is.True);
        }
    }
}