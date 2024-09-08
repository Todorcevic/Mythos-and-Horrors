using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardAdversity01165Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CantPlaySupplyAndCondition()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01165 cardAdversity = _cardsProvider.GetCard<Card01165>();
            Card01506 supply = _cardsProvider.GetCard<Card01506>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<RoundGameAction>().Execute();
            yield return AssertThatIsNotClickable(supply);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
        }

        [UnityTest]
        public IEnumerator CantPlaySupplyAndConditionWithOptianalReaction()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01165 cardAdversity = _cardsProvider.GetCard<Card01165>();
            Card01510 condition = _cardsProvider.GetCard<Card01510>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(condition, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<RoundGameAction>().Execute();
            yield return AssertThatIsNotClickable(condition);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
        }

        [UnityTest]
        public IEnumerator CanCommit()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            Card01165 cardAdversity = _cardsProvider.GetCard<Card01165>();
            Card01534 cardSupplyToCommit = _cardsProvider.GetCard<Card01534>();

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupplyToCommit, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardAdversity, investigator.DangerZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedIn(cardSupplyToCommit);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
        }
    }
}