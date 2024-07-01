
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01690Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator SortCardWithFear()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01690", investigator);
            Card01690 supply = _cardsProvider.GetCard<Card01690>();
            Card card1 = _cardsProvider.GetCard<Card01530>();
            Card card2 = _cardsProvider.GetCard<Card01166>();
            Card card3 = _cardsProvider.GetCard<Card01525>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(new[] { card1, card2, card3 }, SceneCORE1.DangerDeckZone, isFaceDown: true).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Start().AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(supply);
            yield return ClickedIn(SceneCORE1.CardDangerToDraw);
            yield return ClickedIn(card3);
            yield return ClickedIn(card1);
            yield return ClickedIn(card2);
            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(3));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.Charge.Amount.Value, Is.EqualTo(2));
            Assert.That(SceneCORE1.CardDangerToDraw, Is.EqualTo(card2));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }
    }
}
