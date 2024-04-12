using MythosAndHorrors.GameRules;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class DoDamageAndFearGameActionTests : TestBase
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator DamageTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card damageableCard = investigator.AllCards.First(card => card.Info.Code == "01521");
            Card damageableCard2 = investigator.AllCards.First(card => card.Info.Code == "01521" && card != damageableCard);
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard2, investigator.AidZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, amountDamage: 5, amountFear: 1)).AsCoroutine();


            if (DEBUG_MODE) yield return new WaitForSeconds(230);
        }
    }
}
