using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01565Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CancelAdversity()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01565 conditionCard = _cardsProvider.GetCard<Card01565>();
            Card01164 cardAdversity = _cardsProvider.GetCard<Card01164>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute();
            yield return ClickedIn(conditionCard);
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DangerZone.Cards.Any(), Is.False);
        }

        [UnityTest]
        public IEnumerator HallowAdversity()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01565 conditionCard = _cardsProvider.GetCard<Card01565>();
            Card01164 cardAdversity = _cardsProvider.GetCard<Card01164>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DangerZone.Cards.Any(), Is.True);
        }

        [UnityTest]
        public IEnumerator TwoCardsInHand()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            IEnumerable<Card01565> conditionCards = _cardsProvider.GetCards<Card01565>();
            Card01164 cardAdversity = _cardsProvider.GetCard<Card01164>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCards, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute();
            yield return ClickedIn(conditionCards.First());
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DangerZone.Cards.Any(), Is.False);
        }
    }
}
