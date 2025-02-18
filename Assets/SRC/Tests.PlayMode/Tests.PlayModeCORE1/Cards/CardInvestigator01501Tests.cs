using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardInvestigator01501Tests : TestCORE1Preparation
    {
        protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator StarChallengeTokenRevealed()
        {
            CardPlace place = _cardsProvider.GetCard<Card01114>();
            Investigator investigatorToTest = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Fail);
            Task<int> tokenValue = CaptureTokenValue(investigatorToTest);
            yield return PlayThisInvestigator(investigatorToTest, withAvatar: false);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(place, _chaptersProvider.CurrentScene.GetPlaceZone(0, 3)).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigatorToTest, place).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigatorToTest).Execute();
            yield return ClickedIn(place);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(8));
        }

        [UnityTest]
        public IEnumerator DiscoverClueWhenDefeatCreature()
        {
            Investigator investigatorToTest = _investigatorsProvider.First;
            CardCreature cardCreature = SceneCORE1.GhoulSecuaz;
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardCreature, investigatorToTest.DangerZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(cardCreature, investigatorToTest.InvestigatorCard, amountDamage: 5).Execute();
            yield return ClickedIn(investigatorToTest.InvestigatorCard);
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.Keys.Value, Is.EqualTo(1));
        }
    }
}
