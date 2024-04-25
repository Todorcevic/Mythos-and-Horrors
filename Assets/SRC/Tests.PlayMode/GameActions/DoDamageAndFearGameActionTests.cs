using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
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
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard2, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(bulletProof, investigator.AidZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, _preparationScene.SceneCORE1.GhoulSecuaz, amountDamage: 2, amountFear: 1));

            if (!DEBUG_MODE) yield return WaitToClick(bulletProof);
            if (!DEBUG_MODE) yield return WaitToClick(damageableCard);
            while (!gameActionTask.IsCompleted) yield return null;

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(investigator.AidZone.Cards.Count, Is.EqualTo(2));
            Assert.That(((IDamageable)bulletProof).Health.Value, Is.EqualTo(2));
        }
    }
}
