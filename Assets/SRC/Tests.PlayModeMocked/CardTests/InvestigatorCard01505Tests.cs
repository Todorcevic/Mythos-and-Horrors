using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigatorCard01505Tests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;
        protected override string JSON_SAVE_DATA_PATH => "Assets/SRC/Tests.PlayModeCORE1/SaveDataCORE1-2.json";

        /*******************************************************************/
        [UnityTest]
        public IEnumerator StarChallengeTokenRevealed()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Star);
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01505>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            CardSupply AmuletoDeWendy = _cardsProvider.GetCard<Card01514>();
            CardPlace place = _preparationSceneCORE1.SceneCORE1.Cellar;
            Task<int> tokenValue = CaptureTokenValue(investigatorToTest);
            Task<ChallengePhaseGameAction> challengeResolved = CaptureResolvingChallenge();
            yield return _preparationSceneCORE1.PlaceAllScene().AsCoroutine();
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigatorToTest, withResources: false).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(AmuletoDeWendy, investigatorToTest.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            FakeInteractablePresenter.ClickedIn(investigatorToTest.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(0));
            Assert.That(challengeResolved.Result.IsAutoSucceed, Is.True);
            Assert.That(investigatorToTest.Hints.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator ChangeTokenRevealed()
        {
            Task revealToken = MustBeRevealedThisToken(ChallengeTokenType.Value_1);
            revealToken.ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value1));
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01505>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            Task<(int totalTokensAmount, int totalTokensValue)> challengeResolved = CaptureTotalTokensRevelaed();
            yield return _preparationSceneCORE1.PlaceAllScene().AsCoroutine();
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigatorToTest, withResources: false).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            FakeInteractablePresenter.ClickedIn(investigatorToTest.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedIn(cardInvestigator);
            FakeInteractablePresenter.ClickedIn(investigatorToTest.HandZone.Cards.First(card => card.CanBeDiscarded));
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(challengeResolved.Result.totalTokensValue, Is.EqualTo(1));
            Assert.That(investigatorToTest.Hints.Value, Is.EqualTo(1));
            Assert.That(investigatorToTest.DiscardZone.Cards.Count, Is.EqualTo(1));
        }
    }
}
