
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardTalent01589Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Draw()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card01589 cardTalent = _cardsProvider.GetCard<Card01589>();
            Card01167 adversity = _cardsProvider.GetCard<Card01167>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            int amountDeckCards = investigator.DeckZone.Cards.Count;

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardTalent, investigator.HandZone).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new DrawGameAction(investigator, adversity));
            yield return ClickedIn(cardTalent);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DeckZone.Cards.Count, Is.EqualTo(amountDeckCards - 1));
        }

        [UnityTest]
        public IEnumerator MaxLimit()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card01589 cardTalent = _cardsProvider.GetCard<Card01589>();
            Card01589 cardTalent2 = _cardsProvider.GetCards<Card01589>().First(card => card != cardTalent);
            Card01167 adversity = _cardsProvider.GetCard<Card01167>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            int amountDeckCards = investigator.DeckZone.Cards.Count;

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardTalent, investigator.HandZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardTalent2, investigator.HandZone).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new DrawGameAction(investigator, adversity));
            yield return ClickedIn(cardTalent);
            yield return AssertThatIsNotClickable(cardTalent2);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DeckZone.Cards.Count, Is.EqualTo(amountDeckCards - 1));
        }
    }
}
