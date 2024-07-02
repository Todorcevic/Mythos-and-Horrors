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

            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.CurrentPlace.OwnZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulSecuaz, SceneCORE1.Attic).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCreatureGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.CurrentPlace).Start().AsCoroutine();

            Assert.That(SceneCORE1.GhoulSecuaz.CurrentPlace, Is.EqualTo(SceneCORE1.Attic));
        }


        [UnityTest]
        public IEnumerator Discarding()
        {
            Investigator investigator = _investigatorsProvider.Second;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01538 conditionCard = _cardsProvider.GetCard<Card01538>();

            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.CurrentPlace.OwnZone).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
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

            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Attic).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, SceneCORE1.Hallway.OwnZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulSecuaz, SceneCORE1.Attic).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(SceneCORE1.Hallway);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(SceneCORE1.GhoulSecuaz.CurrentPlace, Is.EqualTo(SceneCORE1.Attic));
        }

        [UnityTest]
        public IEnumerator BlockingConfrontedWheneCreatureInto()
        {
            Investigator investigator = _investigatorsProvider.Second;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01538 conditionCard = _cardsProvider.GetCard<Card01538>();

            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Attic).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, SceneCORE1.Hallway.OwnZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulSecuaz, SceneCORE1.Attic).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulVoraz, SceneCORE1.Hallway).Start().AsCoroutine();


            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(SceneCORE1.Hallway);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(SceneCORE1.GhoulSecuaz.CurrentPlace, Is.EqualTo(SceneCORE1.Attic));
            Assert.That(SceneCORE1.GhoulVoraz.CurrentPlace, Is.EqualTo(SceneCORE1.Hallway));
        }
    }
}
