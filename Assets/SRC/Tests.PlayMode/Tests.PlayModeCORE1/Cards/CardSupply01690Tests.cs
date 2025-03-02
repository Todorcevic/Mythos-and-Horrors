
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
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
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card card1 = _chaptersProvider.CurrentScene.DangerCards.SkipLast(1).Last();
            Card card2 = _chaptersProvider.CurrentScene.DangerCards.SkipLast(2).Last();
            Card card3 = _cardsProvider.GetCard<Card01166>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(card3, SceneCORE1.DangerDeckZone, isFaceDown: true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(supply);
            yield return ClickedIn(SceneCORE1.CardDangerToDraw);
            yield return ClickedIn(card3);
            yield return ClickedIn(card1);
            yield return ClickedIn(card2);
            Assert.That(investigator.CurrentActions.Value, Is.EqualTo(3));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.Charge.Amount.Value, Is.EqualTo(2));
            Assert.That(SceneCORE1.CardDangerToDraw, Is.EqualTo(card2));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }
    }
}
