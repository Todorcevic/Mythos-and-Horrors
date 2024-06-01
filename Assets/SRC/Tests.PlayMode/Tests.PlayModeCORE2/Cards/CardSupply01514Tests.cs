
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
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
            Card01579 cardCondition = _cardsProvider.GetCard<Card01579>();

            yield return _gameActionsProvider.Create(new GainResourceGameAction(investigator, 8)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supplyCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardCondition, investigator.HandZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
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

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supplyCard, investigator.AidZone)).AsCoroutine();

            Task<RoundGameAction> taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            yield return ClickedIn(conditionCard);
            Assert.That(conditionCard.CurrentZone, Is.EqualTo(investigator.DeckZone));
            yield return ClickedTokenButton();
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
            Card01579 cardCondition = _cardsProvider.GetCard<Card01579>();

            yield return _gameActionsProvider.Create(new GainResourceGameAction(investigator, 8)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supplyCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardCondition, investigator.DiscardZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
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

            yield return _gameActionsProvider.Create(new GainResourceGameAction(investigator, 8)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supplyCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardCondition, investigator.DiscardZone)).AsCoroutine();

            Task<RoundGameAction> taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            yield return ClickedIn(cardCondition);
            Assert.That(cardCondition.CurrentZone, Is.EqualTo(investigator.DeckZone));
            yield return ClickedTokenButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();
            Assert.That(investigator.Resources.Value, Is.EqualTo(14));
        }
    }
}
