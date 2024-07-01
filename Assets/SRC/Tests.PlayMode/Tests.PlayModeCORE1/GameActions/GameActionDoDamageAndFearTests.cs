using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionDoDamageAndFearTests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator HarmTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01594", investigator);
            Card01594 bulletProof = _cardsProvider.GetCard<Card01594>();
            Card01521 damageableCard = _cardsProvider.GetCard<Card01521>(); ;

            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(damageableCard, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(bulletProof, investigator.AidZone).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, SceneCORE1.GhoulSecuaz, amountDamage: 2, amountFear: 1));
            yield return ClickedIn(bulletProof);
            yield return ClickedIn(damageableCard);
            yield return ClickedIn(damageableCard);
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.AidZone.Cards.Count, Is.EqualTo(1));
            Assert.That(((IDamageable)bulletProof).HealthLeft, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator UndoHarmTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01594", investigator);
            Card bulletProof = _cardsProvider.GetCard<Card01594>();
            Card damageableCard = investigator.Cards.First(card => card.Info.Code == "01521");
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(damageableCard, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(bulletProof, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, SceneCORE1.Hallway)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(SceneCORE1.Attic);
            yield return ClickedIn(damageableCard);
            yield return ClickedUndoButton();
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.AidZone.Cards.Count, Is.EqualTo(2));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }
    }
}
