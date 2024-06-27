
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01561Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator SortCards()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            Card01561 supply = _cardsProvider.GetCard<Card01561>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supply, investigator.AidZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(supply);
            yield return ClickedIn(investigator2.InvestigatorCard);
            yield return ClickedIn(SceneCORE1.LimboZone.Cards.First());
            yield return ClickedIn(SceneCORE1.LimboZone.Cards.First());
            yield return ClickedIn(SceneCORE1.LimboZone.Cards.First());
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.AmountCharges.Value, Is.EqualTo(2));
        }
    }
}
