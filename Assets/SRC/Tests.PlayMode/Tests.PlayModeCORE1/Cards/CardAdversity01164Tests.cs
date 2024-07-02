using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardAdversity01164Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Unit;

        [UnityTest]
        public IEnumerator MoveCostExtraTurn()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01164 cardAdversity = _cardsProvider.GetCard<Card01164>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Start().AsCoroutine();
            Assert.That(cardAdversity.Wasted.IsActive, Is.False);

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(SceneCORE1.Attic);
            //Assert.That(cardAdversity.Wasted.IsActive, Is.True);
            //Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(1));
            yield return ClickedIn(SceneCORE1.Hallway);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(investigator.DangerZone));
            Assert.That(cardAdversity.Wasted.IsActive, Is.False);
        }

        [UnityTest]
        public IEnumerator AttackCostExtraTurn()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01164 cardAdversity = _cardsProvider.GetCard<Card01164>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value1));
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.DangerZone).Start().AsCoroutine();

            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Start().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(SceneCORE1.GhoulSecuaz, 0);
            yield return ClickedMainButton();
            //Assert.That(cardAdversity.Wasted.IsActive, Is.True);
            //Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(1));
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
            Assert.That(cardAdversity.Wasted.IsActive, Is.False);
        }
    }
}