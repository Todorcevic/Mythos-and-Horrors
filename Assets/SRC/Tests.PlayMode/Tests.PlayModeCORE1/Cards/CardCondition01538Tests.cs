using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01538Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Blocking()
        {
            Investigator investigator = _investigatorsProvider.Second;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01538 conditionCard = _cardsProvider.GetCard<Card01538>();

            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator.CurrentPlace.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(SceneCORE1.GhoulSecuaz, SceneCORE1.Attic)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCreatureGameAction(SceneCORE1.GhoulSecuaz, investigator.CurrentPlace)).AsCoroutine();

            Assert.That(SceneCORE1.GhoulSecuaz.CurrentPlace, Is.EqualTo(SceneCORE1.Attic));
        }


        [UnityTest]
        public IEnumerator Discarding()
        {
            Investigator investigator = _investigatorsProvider.Second;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01538 conditionCard = _cardsProvider.GetCard<Card01538>();

            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator.CurrentPlace.OwnZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(SceneCORE1.Attic);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(conditionCard.CurrentZone, Is.EqualTo(conditionCard.Owner.DiscardZone));
        }

        [UnityTest]
        public IEnumerator BlockingConfronted()
        {
            Investigator investigator = _investigatorsProvider.Second;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01538 conditionCard = _cardsProvider.GetCard<Card01538>();

            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, SceneCORE1.Attic)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, SceneCORE1.Hallway.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(SceneCORE1.GhoulSecuaz, SceneCORE1.Attic)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(SceneCORE1.Hallway);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(SceneCORE1.GhoulSecuaz.CurrentPlace, Is.EqualTo(SceneCORE1.Attic));
        }
    }
}
