using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CardPlace01115Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator Resign()
        {
            CardPlace Parlor = _cardsProvider.GetCard<Card01115>();
            Investigator investigator = _investigatorsProvider.First;
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, Parlor)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(Parlor, 1);
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Resign.IsActive, Is.True);
            Assert.That(investigator.InvestigatorCard.IsInPlay, Is.False);
        }

        [UnityTest]
        public IEnumerator Parley()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);
            CardPlace Parlor = _cardsProvider.GetCard<Card01115>();
            Investigator investigator = _investigatorsProvider.Second;
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.Lita, Parlor.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, Parlor)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(Parlor, 2);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.AidZone.Cards.Contains(SceneCORE1.Lita), Is.True);
        }
    }
}
