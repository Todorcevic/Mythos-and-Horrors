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

            Card card1 = investigator2.DeckZone.Cards.Last();
            Card card2 = investigator2.DeckZone.Cards.SkipLast(1).Last();
            Card card3 = investigator2.DeckZone.Cards.SkipLast(2).Last();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(supply);
            yield return ClickedIn(investigator2.InvestigatorCard);
            yield return ClickedIn(card3);
            yield return ClickedIn(card1);
            yield return ClickedIn(card2);
            yield return AssertThatIsNotClickable(supply);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.Exausted.IsActive, Is.True);
            Assert.That(supply.Charge.Amount.Value, Is.EqualTo(2));
            Assert.That(investigator2.CardAidToDraw, Is.EqualTo(card2));
        }
    }
}
