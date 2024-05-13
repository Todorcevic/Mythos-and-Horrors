using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigatorCard01501Tests : TestCORE1PlayModeBase
    {
        [UnityTest]
        public IEnumerator StarChallengeTokenRevealed()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Star);

            CardPlace place = _cardsProvider.GetCard<Card01114>();
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01501>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            Task<int> tokenValue = CaptureTokenValue(investigatorToTest);
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigatorToTest, withAvatar: false).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            FakeInteractablePresenter.ClickedIn(place);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(8));
        }

        [UnityTest]
        public IEnumerator DiscoverClueWhenDefeatCreature()
        {
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01501>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            CardCreature cardCreature = _preparationSceneCORE1.SceneCORE1.GhoulSecuaz;
            yield return _preparationSceneCORE1.StartingScene().AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardCreature, investigatorToTest.DangerZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new HarmToCardGameAction(cardCreature, investigatorToTest.InvestigatorCard, amountDamage: 5));
            FakeInteractablePresenter.ClickedIn(investigatorToTest.InvestigatorCard);
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.Hints.Value, Is.EqualTo(1));
        }
    }
}
