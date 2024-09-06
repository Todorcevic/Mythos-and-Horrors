using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{

    public class CardCondition01510Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ActivateToCancelAttackCreature()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator, withResources: true);
            Card01510 conditionCard = _cardsProvider.GetCard<Card01510>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.DangerZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<RoundGameAction>().Execute();
            yield return ClickedIn(conditionCard);
            yield return ClickedResourceButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(0));
        }
    }
}
