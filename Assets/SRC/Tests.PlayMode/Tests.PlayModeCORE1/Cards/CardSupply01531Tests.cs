
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01531Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ShowAndDrawCard()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            Card01531 cardSupply = _cardsProvider.GetCard<Card01531>();
            Card01521 cardAsset = _cardsProvider.GetCard<Card01521>();
            Card01520 cardAsset2 = _cardsProvider.GetCard<Card01520>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardAsset, investigator2.DeckZone, isFaceDown: true).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardAsset2, investigator2.DeckZone, isFaceDown: true).Start().AsCoroutine();


            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(cardSupply);
            yield return ClickedIn(investigator2.InvestigatorCard);
            yield return ClickedIn(cardAsset);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(cardAsset.CurrentZone, Is.EqualTo(investigator2.HandZone));
            Assert.That(cardAsset.FaceDown.IsActive, Is.False);
            Assert.That(cardAsset2.FaceDown.IsActive, Is.True);
        }
    }
}
