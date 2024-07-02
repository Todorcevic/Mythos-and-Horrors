
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

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardAsset, investigator2.DeckZone, isFaceDown: true).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardAsset2, investigator2.DeckZone, isFaceDown: true).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(cardSupply);
            yield return ClickedIn(investigator2.InvestigatorCard);
            yield return ClickedIn(cardAsset);
            yield return ClickedIn(cardSupply);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator2.Resources.Value, Is.EqualTo(4));
            Assert.That(cardAsset.CurrentZone, Is.EqualTo(investigator2.AidZone));
            Assert.That(cardAsset.FaceDown.IsActive, Is.False);
            Assert.That(cardAsset2.FaceDown.IsActive, Is.True);
        }

        [UnityTest]
        public IEnumerator ShowAndDrawCardConditionCantPlay()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            yield return BuildCard("01686", investigator);
            yield return BuildCard("01526", investigator2);

            Card01686 cardSupply = _cardsProvider.GetCard<Card01686>();
            Card01526 cardCondition = _cardsProvider.GetCard<Card01526>();
            Card01520 cardAsset2 = _cardsProvider.GetCard<Card01520>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardCondition, investigator2.DeckZone, isFaceDown: true).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardAsset2, investigator2.DeckZone, isFaceDown: true).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(cardSupply);
            yield return ClickedIn(investigator2.InvestigatorCard);
            yield return ClickedIn(cardCondition);
            yield return AssertThatIsNotClickable(cardSupply);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(cardCondition.FaceDown.IsActive, Is.False);
            Assert.That(cardAsset2.FaceDown.IsActive, Is.True);
        }

        [UnityTest]
        public IEnumerator ShowAndDrawCardConditionCanPlay()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            yield return BuildCard("01686", investigator);
            yield return BuildCard("01557", investigator2);

            Card01686 cardSupply = _cardsProvider.GetCard<Card01686>();
            Card01557 cardCondition = _cardsProvider.GetCard<Card01557>();
            Card01520 cardAsset2 = _cardsProvider.GetCard<Card01520>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardCondition, investigator2.DeckZone, isFaceDown: true).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardAsset2, investigator2.DeckZone, isFaceDown: true).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(cardSupply);
            yield return ClickedIn(investigator2.InvestigatorCard);
            yield return ClickedIn(cardCondition);
            yield return ClickedIn(cardSupply);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator2.Resources.Value, Is.EqualTo(14));
            Assert.That(cardCondition.CurrentZone, Is.EqualTo(investigator2.DiscardZone));
            Assert.That(cardCondition.FaceDown.IsActive, Is.False);
            Assert.That(cardAsset2.FaceDown.IsActive, Is.True);
        }

        //[UnityTest]
        //public IEnumerator ShowAndDrawCardConditionCanPlay3()
        //{
        //    Investigator investigator = _investigatorsProvider.First;
        //    Investigator investigator2 = _investigatorsProvider.Second;
        //    yield return BuildCard("01686", investigator);
        //    yield return BuildCard("01551", investigator2);

        //    Card01686 cardSupply = _cardsProvider.GetCard<Card01686>();
        //    Card01551 cardCondition = _cardsProvider.GetCard<Card01551>();
        //    Card01520 cardAsset2 = _cardsProvider.GetCard<Card01520>();
        //    yield return PlaceOnlyScene();
        //    yield return PlayThisInvestigator(investigator);
        //    yield return PlayThisInvestigator(investigator2);

        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardSupply, investigator.AidZone)).AsCoroutine();
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardCondition, investigator2.DeckZone, isFaceDown: true)).AsCoroutine();
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardAsset2, investigator2.DeckZone, isFaceDown: true)).AsCoroutine();
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulSecuaz, investigator2.DangerZone)).AsCoroutine();


        //    Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
        //    yield return ClickedIn(cardSupply);
        //    yield return ClickedIn(investigator2.InvestigatorCard);
        //    yield return ClickedIn(cardCondition);
        //    yield return AssertThatIsNotClickable(cardSupply);
        //    //yield return ClickedIn(cardSupply);
        //    yield return ClickedMainButton();
        //    yield return gameActionTask.AsCoroutine();

        //    //Assert.That(investigator2.Resources.Value, Is.EqualTo(14));
        //    //Assert.That(cardCondition.CurrentZone, Is.EqualTo(investigator2.DiscardZone));
        //    Assert.That(cardCondition.FaceDown.IsActive, Is.False);
        //    Assert.That(cardAsset2.FaceDown.IsActive, Is.True);
        //}
    }
}
