
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
            Card01578 cardCondition = _cardsProvider.GetCard<Card01578>();

            yield return _gameActionsProvider.Create<GainResourceGameAction>().SetWith(investigator, 8).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supplyCard, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardCondition, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.Drew, investigator.DangerZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(cardCondition);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(cardCondition.CurrentZone, Is.EqualTo(investigator.DeckZone));
        }

        [UnityTest]
        public IEnumerator DiscardOptativeCondition()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator, withResources: true);
            Card01510 conditionCard = _cardsProvider.GetCard<Card01510>();
            Card01514 supplyCard = _cardsProvider.GetCard<Card01514>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supplyCard, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<RoundGameAction>().Execute();
            yield return ClickedIn(conditionCard);
            Assert.That(conditionCard.CurrentZone, Is.EqualTo(investigator.DeckZone));
            yield return ClickedResourceButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Resources.Value, Is.EqualTo(6));
        }

        [UnityTest]
        public IEnumerator PlayFromDiscardPile()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01514 supplyCard = _cardsProvider.GetCard<Card01514>();
            Card01578 cardCondition = _cardsProvider.GetCard<Card01578>();

            yield return _gameActionsProvider.Create<GainResourceGameAction>().SetWith(investigator, 8).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supplyCard, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardCondition, investigator.DiscardZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.Drew, investigator.DangerZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(cardCondition);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(cardCondition.CurrentZone, Is.EqualTo(investigator.DeckZone));
        }

        [UnityTest]
        public IEnumerator PlayFromDiscardPileOptative()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01514 supplyCard = _cardsProvider.GetCard<Card01514>();
            Card01510 cardCondition = _cardsProvider.GetCard<Card01510>();
            Card01580 basicCard = _cardsProvider.GetCard<Card01580>();

            yield return _gameActionsProvider.Create<GainResourceGameAction>().SetWith(investigator, 8).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(basicCard, investigator.DeckZone, isFaceDown: true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supplyCard, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardCondition, investigator.DiscardZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<RoundGameAction>().Execute();
            yield return ClickedIn(cardCondition);
            Assert.That(cardCondition.CurrentZone, Is.EqualTo(investigator.DeckZone));
            yield return ClickedResourceButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();
            Assert.That(investigator.Resources.Value, Is.EqualTo(14));
        }

        [UnityTest]
        public IEnumerator PlayFromDiscardPileOptativeDiscoverKey()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01514 supplyCard = _cardsProvider.GetCard<Card01514>();
            Card01522 cardCondition = _cardsProvider.GetCard<Card01522>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supplyCard, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardCondition, investigator.DiscardZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.Drew, investigator.DangerZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<DefeatCardGameAction>().SetWith(SceneCORE2.Drew, investigator.InvestigatorCard).Execute();
            yield return ClickedIn(cardCondition);
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator PlayFromDiscardPileOptativeSpecificCondition()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuildCard("01565", investigator);
            Card01514 supplyCard = _cardsProvider.GetCard<Card01514>();
            Card01565 cardCondition = _cardsProvider.GetCard<Card01565>();
            Card01168 cardAdversity = _cardsProvider.GetCard<Card01168>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supplyCard, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardCondition, investigator.DiscardZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute();
            yield return ClickedIn(cardCondition);
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.DangerZone.Cards.Any(), Is.False);
        }
    }
}
