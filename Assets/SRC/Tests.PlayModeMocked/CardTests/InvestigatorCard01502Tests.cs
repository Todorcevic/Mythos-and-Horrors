using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigatorCard01502Tests : TestCORE1PlayModeBase
    {
        protected override bool DEBUG_MODE => true;

        [UnityTest]
        public IEnumerator StarChallengeTokenRevealed()
        {
            Card tomeCard = _cardsProvider.GetCard<Card01531>();
            Card tomeCard2 = _cardsProvider.GetCard<Card01535>();
            CardPlace place = _preparationSceneCORE1.SceneCORE1.Cellar;
            Investigator investigatorToTest = _investigatorsProvider.Second;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Star);
            yield return _preparationSceneCORE1.StartingScene().AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard, investigatorToTest.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard2, investigatorToTest.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();
            int resultExpected = investigatorToTest.DeckZone.Cards.Count - 2;
           
            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            FakeInteractablePresenter.ClickedIn(place);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.DeckZone.Cards.Count, Is.EqualTo(resultExpected));
        }

        [UnityTest]
        public IEnumerator FreeTurnToActivateTome()
        {
            Card tomeCard = _cardsProvider.GetCard<Card01535>();
            Investigator investigatorToTest = _investigatorsProvider.Second;
            yield return _preparationSceneCORE1.StartingScene().AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard, investigatorToTest.AidZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            FakeInteractablePresenter.ClickedIn(investigatorToTest.InvestigatorCard);
            FakeInteractablePresenter.ClickedIn(tomeCard);
            FakeInteractablePresenter.ClickedIn(investigatorToTest.AvatarCard);
            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(3));

            FakeInteractablePresenter.ClickedTokenButton();
            FakeInteractablePresenter.ClickedTokenButton();
            FakeInteractablePresenter.ClickedTokenButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.Resources.Value, Is.EqualTo(3));
            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(0));
        }
    }
}
