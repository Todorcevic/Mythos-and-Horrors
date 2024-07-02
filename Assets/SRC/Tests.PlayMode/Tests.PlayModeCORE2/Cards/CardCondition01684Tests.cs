using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCondition01684Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator AvoidHarm()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuildCard("01684", investigator);
            Card01684 conditionCard = _cardsProvider.GetCard<Card01684>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, SceneCORE2.Drew, amountDamage: 2, amountFear: 3).Start();
            yield return ClickedIn(conditionCard);
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Resources.Value, Is.EqualTo(10));
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(0));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(0));
        }
    }
}
