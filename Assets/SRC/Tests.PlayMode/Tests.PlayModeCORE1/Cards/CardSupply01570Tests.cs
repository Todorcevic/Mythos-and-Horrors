
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01570Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator GainCharge()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return BuildCard("01570", investigator);
            Card01570 supply = _cardsProvider.GetCard<Card01570>();
            Card01561 spell = _cardsProvider.GetCard<Card01561>();

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(spell, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(supply);
            yield return ClickedIn(supply);
            yield return ClickedIn(spell);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.Exausted.IsActive, Is.True);
            Assert.That(spell.Charge.Amount.Value, Is.EqualTo(4));
            Assert.That(investigator.SlotsCollection.AllSlotsType.Count(slot => slot == SlotType.Magical), Is.EqualTo(3));
        }
    }
}
