using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01522Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ActivateToDiscoverHint()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator, withResources: true);
            Card01522 conditionCard = _cardsProvider.GetCard<Card01522>();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new DefeatCardGameAction(SceneCORE1.GhoulSecuaz, investigator.InvestigatorCard));
            yield return ClickedIn(conditionCard);
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
        }
    }
}
