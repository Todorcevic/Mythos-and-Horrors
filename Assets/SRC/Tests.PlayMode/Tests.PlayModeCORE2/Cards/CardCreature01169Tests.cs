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
            Card01169 acolit = SceneCORE2.Acolits.First();

            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(acolit)).AsCoroutine();

            Assert.That(acolit.Eldritch.Value, Is.EqualTo(1));
            Assert.That(SceneCORE2.CurrentPlot.Eldritch.Value, Is.EqualTo(6));
        }

        [UnityTest]
        public IEnumerator SpawnInAlonePlace()
        {
            yield return StartingScene();
            CardCreature acolit = SceneCORE2.Acolits.First();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(acolit, SceneCORE2.DangerDeckZone, isFaceDown: true).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.First, SceneCORE2.Home).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE2.Fluvial).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Third, SceneCORE2.Hospital).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Fourth, SceneCORE2.North).Start().AsCoroutine();

            yield return _gameActionsProvider.Create(new DrawDangerGameAction(_investigatorsProvider.First)).AsCoroutine();

            Assert.That(acolit.IsInPlay, Is.True);
            Assert.That(acolit.CurrentZone.ZoneType, Is.Not.EqualTo(ZoneType.Danger));
            Assert.That(_investigatorsProvider.First.CurrentPlace.DistanceTo(acolit.CurrentPlace).distance, Is.EqualTo(2));

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(acolit, SceneCORE2.DangerDeckZone, isFaceDown: true).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE2.North).Start().AsCoroutine();

            yield return _gameActionsProvider.Create(new DrawDangerGameAction(_investigatorsProvider.First)).AsCoroutine();

            Assert.That(_investigatorsProvider.First.CurrentPlace.DistanceTo(acolit.CurrentPlace).distance, Is.EqualTo(1));
            Assert.That(acolit.CurrentPlace, Is.EqualTo(SceneCORE2.Fluvial));
        }
    }
}
