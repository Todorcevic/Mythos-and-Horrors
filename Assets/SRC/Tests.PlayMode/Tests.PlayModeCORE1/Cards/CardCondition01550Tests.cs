using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01550Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DeconfrontAndMove()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01550 conditionCard = _cardsProvider.GetCard<Card01550>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.CurrentPlace).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulVoraz, SceneCORE1.Attic).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(conditionCard);
            Assert.That(SceneCORE1.Attic.CanBePlayed.IsTrue, Is.False);
            Assert.That(SceneCORE1.Cellar.CanBePlayed.IsTrue, Is.True);
            yield return ClickedIn(SceneCORE1.Cellar);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE1.Cellar));
            Assert.That(SceneCORE1.GhoulSecuaz.CurrentPlace, Is.EqualTo(SceneCORE1.Hallway));
        }
    }
}
