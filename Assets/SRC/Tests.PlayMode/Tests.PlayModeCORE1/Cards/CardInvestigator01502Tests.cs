using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardInvestigator01502Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator StarChallengeTokenRevealed()
        {
            Card tomeCard = _cardsProvider.GetCard<Card01531>();
            Card tomeCard2 = _cardsProvider.GetCard<Card01535>();
            Card01509 necro = _cardsProvider.GetCard<Card01509>();
            CardPlace place = SceneCORE1.Cellar;
            Investigator investigatorToTest = _investigatorsProvider.Second;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Star);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigatorToTest);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(necro, SceneCORE1.OutZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard, investigatorToTest.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard2, investigatorToTest.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();
            int resultExpected = investigatorToTest.DeckZone.Cards.Count - 2;

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            yield return ClickedIn(place);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.DeckZone.Cards.Count, Is.EqualTo(resultExpected));
        }

        [UnityTest]
        public IEnumerator FreeTurnToActivateTome()
        {
            Card01535 tomeCard = _cardsProvider.GetCard<Card01535>();
            Investigator investigatorToTest = _investigatorsProvider.Second;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard, investigatorToTest.AidZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            yield return ClickedIn(investigatorToTest.InvestigatorCard);
            yield return ClickedIn(tomeCard);
            yield return ClickedIn(investigatorToTest.InvestigatorCard);
            yield return ClickedMainButton();
            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(3));
            yield return ClickedTokenButton();
            yield return ClickedTokenButton();
            yield return ClickedTokenButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.Resources.Value, Is.EqualTo(3));
            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator CancelFreeTurnToActivateTome()
        {
            Card tomeCard = _cardsProvider.GetCard<Card01535>();
            Investigator investigatorToTest = _investigatorsProvider.Second;
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tomeCard, investigatorToTest.AidZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            yield return ClickedIn(investigatorToTest.InvestigatorCard);
            yield return ClickedUndoButton();

            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(3));

            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(0));
        }
    }
}
