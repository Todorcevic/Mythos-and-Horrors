using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Linq;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{

    public class CardCondition01523Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CancelAttack()
        {
            Investigator investigator = _investigatorsProvider.Third;
            Investigator investigator2 = _investigatorsProvider.Second;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator, withResources: true);
            yield return PlayThisInvestigator(investigator2);
            Card01523 conditionCard = _cardsProvider.GetCard<Card01523>();
            Card01523 conditionCard2 = _cardsProvider.GetCards<Card01523>().First(card => card != conditionCard);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard2, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.DangerZone).Execute().AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create<CreatureAttackGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator2).Execute();
            yield return ClickedIn(conditionCard);
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator2.DamageRecived.Value, Is.EqualTo(0));
        }
    }
}
