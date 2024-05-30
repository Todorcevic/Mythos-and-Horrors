
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardSupply01514Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ConditionDiscardToLastDeck()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01514 supplyCard = _cardsProvider.GetCard<Card01514>();
            Card01522 cardCondition = _cardsProvider.GetCard<Card01522>();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supplyCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardCondition, investigator.HandZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(cardCondition);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(cardCondition.CurrentZone, Is.EqualTo(investigator.DeckZone));
        }
    }
}
