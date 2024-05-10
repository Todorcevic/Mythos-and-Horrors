using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class DoDamageAndFearGameActionTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator DamageTest()
        {
            Card bulletProof = _cardLoaderUseCase.Execute("01594");
            _cardViewGeneratorComponent.BuildCardView(bulletProof);
            Investigator investigator = _investigatorsProvider.First;
            Card damageableCard = investigator.AllCards.First(card => card.Info.Code == "01521");
            Card damageableCard2 = investigator.AllCards.First(card => card.Info.Code == "01521" && card != damageableCard);
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard2, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(bulletProof, investigator.AidZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, _preparationSceneCORE1.SceneCORE1.GhoulSecuaz, amountDamage: 2, amountFear: 1));

            if (!DEBUG_MODE) yield return WaitToClick(bulletProof);
            if (!DEBUG_MODE) yield return WaitToClick(damageableCard);
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.AidZone.Cards.Count, Is.EqualTo(2));
            Assert.That(((IDamageable)bulletProof).Health.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator UndoDamageTest()
        {
            Card bulletProof = _cardLoaderUseCase.Execute("01594");
            _cardViewGeneratorComponent.BuildCardView(bulletProof);
            Investigator investigator = _investigatorsProvider.First;
            Card damageableCard = investigator.AllCards.First(card => card.Info.Code == "01521");
            Card damageableCard2 = investigator.AllCards.First(card => card.Info.Code == "01521" && card != damageableCard);
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard2, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(bulletProof, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, _preparationSceneCORE1.SceneCORE1.Hallway)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(_preparationSceneCORE1.SceneCORE1.Attic);
            if (!DEBUG_MODE) yield return WaitToClick(damageableCard);
            if (!DEBUG_MODE) yield return WaitToUndoClick();
            if (!DEBUG_MODE) yield return WaitToClick(investigator.InvestigatorCard);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.AidZone.Cards.Count, Is.EqualTo(3));
            Assert.That(investigator.Sanity.Value, Is.EqualTo(4));
        }
    }
}
