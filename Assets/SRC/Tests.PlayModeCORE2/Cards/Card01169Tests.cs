using MythosAndHorrors.GameRules;
using System.Linq;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01169Tests : TestBase
    {
        protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator GainEldritch()
        {
            yield return _preparationSceneCORE2.PlaceAllScene();
            CardCreature acolit = _preparationSceneCORE2.SceneCORE2.Acolits.First();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, _preparationSceneCORE2.SceneCORE2.Fluvial.OwnZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(acolit.Eldritch.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator SpawnInAlonePlace()
        {
            yield return _preparationSceneCORE2.StartingScene();
            CardCreature acolit = _preparationSceneCORE2.SceneCORE2.Acolits.First();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, _preparationSceneCORE2.SceneCORE2.DangerDeckZone, isFaceDown: true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationSceneCORE2.SceneCORE2.Home)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Second, _preparationSceneCORE2.SceneCORE2.Fluvial)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Third, _preparationSceneCORE2.SceneCORE2.Hospital)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Fourth, _preparationSceneCORE2.SceneCORE2.North)).AsCoroutine();

            yield return _gameActionsProvider.Create(new DrawDangerGameAction(_investigatorsProvider.First)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(acolit.IsInPlay, Is.True);
            Assert.That(acolit.CurrentZone.ZoneType, Is.Not.EqualTo(ZoneType.Danger));
            Assert.That(_investigatorsProvider.First.CurrentPlace.DistanceTo(acolit.CurrentPlace), Is.EqualTo(2));

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, _preparationSceneCORE2.SceneCORE2.DangerDeckZone, isFaceDown: true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Second, _preparationSceneCORE2.SceneCORE2.North)).AsCoroutine();

            yield return _gameActionsProvider.Create(new DrawDangerGameAction(_investigatorsProvider.First)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(_investigatorsProvider.First.CurrentPlace.DistanceTo(acolit.CurrentPlace), Is.EqualTo(1));
            Assert.That(acolit.CurrentPlace, Is.EqualTo(_preparationSceneCORE2.SceneCORE2.Fluvial));
        }
    }
}
