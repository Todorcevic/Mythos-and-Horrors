using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01524Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Explote()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Third;

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);
            Card01524 conditionCard = _cardsProvider.GetCard<Card01524>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator2, SceneCORE1.Attic).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator2.DangerZone).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(SceneCORE1.Attic);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator2.DamageRecived.Value, Is.EqualTo(3));
            Assert.That(SceneCORE1.GhoulSecuaz.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
        }
    }
}
