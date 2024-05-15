using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigateGameActionTests : TestCORE1PlayModeBase
    {
        [UnityTest]
        public IEnumerator InvestigatePlace()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);
            yield return _preparationSceneCORE1.PlayThisInvestigator(_investigatorsProvider.First).AsCoroutine();
            CardPlace place = _cardsProvider.GetCard<Card01111>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, place)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            FakeInteractablePresenter.ClickedIn(place);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();

            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.First.Hints.Value, Is.EqualTo(1));
        }
    }
}
