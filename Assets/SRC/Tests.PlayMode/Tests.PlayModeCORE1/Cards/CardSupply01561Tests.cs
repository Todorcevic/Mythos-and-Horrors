using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
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

            Card card1 = _cardsProvider.GetCard<Card01530>();
            Card card2 = _cardsProvider.GetCard<Card01522>();
            Card card3 = _cardsProvider.GetCard<Card01525>();

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(new[] { card1, card2, card3 }, investigator2.DeckZone, isFaceDown: true).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Start().AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(supply);
            yield return ClickedIn(investigator2.InvestigatorCard);
            yield return ClickedIn(card3);
            yield return ClickedIn(card1);
            //yield return ClickedIn(card2);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.Charge.Amount.Value, Is.EqualTo(2));
            Assert.That(investigator2.CardAidToDraw, Is.EqualTo(card2));
        }
    }
}
