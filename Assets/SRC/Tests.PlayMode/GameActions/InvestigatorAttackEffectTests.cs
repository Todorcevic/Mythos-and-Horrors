using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigatorAttackEffectTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InvestigatorAttackInDangerZoneTest()
        {
            CardCreature creature = _preparationScene.SceneCORE1.GhoulSecuaz;

            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _investigatorsProvider.First.DangerZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(creature);
            if (!DEBUG_MODE) yield return WaitToCloneClick(0);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!gameActionTask.IsCompleted) yield return null;
            Assert.That(creature.Health.Value, Is.EqualTo(creature.Info.Health - 1));
        }

        [UnityTest]
        public IEnumerator InvestigatorAttackInPlaceZoneTest()
        {
            CardCreature creature = _preparationScene.SceneCORE1.GhoulSecuaz;
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _investigatorsProvider.First.CurrentPlace.OwnZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(creature);
            if (!DEBUG_MODE) yield return WaitToCloneClick(0);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!gameActionTask.IsCompleted) yield return null;
            Assert.That(creature.Health.Value, Is.EqualTo(creature.Info.Health - 1));
        }

        //[UnityTest]
        //public IEnumerator CantInvestigatorAttackTest()
        //{
        //    CardCreature creature = _cardsProvider.AllCards.OfType<CardCreature>().First();
        //    CardPlace place = _cardsProvider.AllCards.OfType<CardPlace>().First();
        //    yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, place.OwnZone)).AsCoroutine();

        //    PlayInvestigatorGameAction oiGA = new(_investigatorsProvider.First);
        //    _ = _gameActionsProvider.Create(oiGA);

        //    if (DEBUG_MODE) yield return new WaitForSeconds(230);
        //    Assert.That(!oiGA.InvestigatorAttackEffects.Exists(effect => effect.Card == creature));
        //}
    }
}
