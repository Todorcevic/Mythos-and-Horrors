
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01686Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ShowAndDrawCardWithDecrease()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            yield return BuildCard("01686", investigator);

            Card01686 cardSupply = _cardsProvider.GetCard<Card01686>();
            Card01521 cardAsset = _cardsProvider.GetCard<Card01521>();
            Card01520 cardAsset2 = _cardsProvider.GetCard<Card01520>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardSupply, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardAsset, investigator2.DeckZone, isFaceDown: true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardAsset2, investigator2.DeckZone, isFaceDown: true)).AsCoroutine();

            int resopurceCostExpected = cardAsset.ResourceCost.Value - 2;
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(cardSupply);
            yield return ClickedIn(investigator2.InvestigatorCard);
            yield return ClickedIn(cardAsset);
            yield return ClickedIn(cardSupply);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(cardAsset.ResourceCost.Value, Is.EqualTo(resopurceCostExpected));
            Assert.That(cardAsset.CurrentZone, Is.EqualTo(investigator2.HandZone));
            Assert.That(cardAsset.FaceDown.IsActive, Is.False);
            Assert.That(cardAsset2.FaceDown.IsActive, Is.True);
        }
    }
}
