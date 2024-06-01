using MythosAndHorrors.GameRules;
using System.Linq;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{

    public class CardCreature01169Tests : TestCORE2Preparation
    {
        [UnityTest]
        public IEnumerator GainEldritch()
        {
            yield return PlaceOnlyScene();
            CardCreature acolit = SceneCORE2.Acolits.First();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, SceneCORE2.Fluvial.OwnZone)).AsCoroutine();

            Assert.That(acolit.Eldritch.Value, Is.EqualTo(1));
            Assert.That(SceneCORE2.CurrentPlot.Eldritch.Value, Is.EqualTo(6));
        }

        [UnityTest]
        public IEnumerator SpawnInAlonePlace()
        {
            yield return StartingScene();
            CardCreature acolit = SceneCORE2.Acolits.First();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, SceneCORE2.DangerDeckZone, isFaceDown: true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, SceneCORE2.Home)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Second, SceneCORE2.Fluvial)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Third, SceneCORE2.Hospital)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Fourth, SceneCORE2.North)).AsCoroutine();

            yield return _gameActionsProvider.Create(new DrawDangerGameAction(_investigatorsProvider.First)).AsCoroutine();

            Assert.That(acolit.IsInPlay, Is.True);
            Assert.That(acolit.CurrentZone.ZoneType, Is.Not.EqualTo(ZoneType.Danger));
            Assert.That(_investigatorsProvider.First.CurrentPlace.DistanceTo(acolit.CurrentPlace).distance, Is.EqualTo(2));

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, SceneCORE2.DangerDeckZone, isFaceDown: true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Second, SceneCORE2.North)).AsCoroutine();

            yield return _gameActionsProvider.Create(new DrawDangerGameAction(_investigatorsProvider.First)).AsCoroutine();

            Assert.That(_investigatorsProvider.First.CurrentPlace.DistanceTo(acolit.CurrentPlace).distance, Is.EqualTo(1));
            Assert.That(acolit.CurrentPlace, Is.EqualTo(SceneCORE2.Fluvial));
        }
    }
}
