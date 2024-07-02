
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01541Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DiscardCreature()
        {
            Investigator investigator = _investigatorsProvider.Second;
            yield return BuildCard("01541", investigator);
            Card01541 supply = _cardsProvider.GetCard<Card01541>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulVoraz, investigator.CurrentPlace).Execute();
            yield return ClickedIn(supply);
            yield return taskGameAction.AsCoroutine();

            Assert.That(SceneCORE1.GhoulVoraz.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
            Assert.That(supply.CurrentZone, Is.EqualTo(investigator.DiscardZone));
        }
    }
}
