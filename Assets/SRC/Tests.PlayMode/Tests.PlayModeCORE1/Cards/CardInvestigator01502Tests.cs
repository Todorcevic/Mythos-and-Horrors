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
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01502>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Star);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigatorToTest);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(necro, SceneCORE1.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(tomeCard, investigatorToTest.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(tomeCard2, investigatorToTest.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigatorToTest, place).Execute().AsCoroutine();
            int resultExpected = investigatorToTest.DeckZone.Cards.Count - 2;

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigatorToTest).Execute();
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
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigatorToTest, withResources: false);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(tomeCard, investigatorToTest.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigatorToTest).Execute();
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
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigatorToTest);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(tomeCard, investigatorToTest.AidZone).Execute().AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigatorToTest).Execute();
            yield return ClickedIn(investigatorToTest.InvestigatorCard);
            yield return ClickedUndoButton();
            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(3));

            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator CantActivateWhenTomeIsExhaust()
        {
            Card tomeCard = _cardsProvider.GetCard<Card01531>();
            Card customCard = _cardsProvider.GetCard<Card01535>();
            Investigator investigatorToTest = _investigatorsProvider.Second;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigatorToTest);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(customCard, investigatorToTest.DeckZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(tomeCard, investigatorToTest.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigatorToTest).Execute();
            yield return ClickedTokenButton();
            yield return ClickedIn(tomeCard);
            yield return ClickedIn(investigatorToTest.InvestigatorCard);
            yield return ClickedIn(customCard);
            yield return ClickedTokenButton();
            yield return AssertThatIsNotClickable(investigatorToTest.InvestigatorCard);
            Assert.That(investigatorToTest.CurrentTurns.Value, Is.EqualTo(0));

            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tomeCard.Exausted.IsActive, Is.True);
        }
    }
}
