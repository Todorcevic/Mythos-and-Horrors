
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01559Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator BuffPower()
        {
            Investigator investigator = _investigatorsProvider.Third;
            Card01559 supply = _cardsProvider.GetCard<Card01559>();

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            int expectedPower = investigator.Power.Value + 1;
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supply, investigator.AidZone)).AsCoroutine();

            Assert.That(investigator.Power.Value, Is.EqualTo(expectedPower));
        }
    }
}
