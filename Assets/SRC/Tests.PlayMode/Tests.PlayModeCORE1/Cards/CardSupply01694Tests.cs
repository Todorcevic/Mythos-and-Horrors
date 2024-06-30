
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01694Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ExtraSupportSlot()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01694", investigator);
            Card01694 supply = _cardsProvider.GetCard<Card01694>();

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supply, investigator.AidZone)).AsCoroutine();

            Assert.That(investigator.SlotsCollection.AllSlotsType.Count(slot => slot == SlotType.Supporter), Is.EqualTo(2));
        }
    }
}
