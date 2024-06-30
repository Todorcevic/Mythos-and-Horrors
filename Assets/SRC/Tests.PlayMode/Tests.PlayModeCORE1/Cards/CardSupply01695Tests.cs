
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01695Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ExtraTrinketSlot()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01695", investigator);
            Card01695 supply = _cardsProvider.GetCard<Card01695>();

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supply, investigator.AidZone)).AsCoroutine();

            Assert.That(investigator.SlotsCollection.AllSlotsType.Count(slot => slot == SlotType.Trinket), Is.EqualTo(2));
        }
    }
}
