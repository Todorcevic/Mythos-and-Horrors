using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardPlace01115Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Resign()
        {
            CardPlace Parlor = _cardsProvider.GetCard<Card01115>();
            Investigator investigator = _investigatorsProvider.First;
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Parlor, _chaptersProvider.CurrentScene.GetPlaceZone(1, 3)).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, Parlor).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(Parlor, 1);
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Resign.IsActive, Is.True);
            Assert.That(investigator.InvestigatorCard.IsInPlay.IsTrue, Is.False);
        }

        [UnityTest]
        public IEnumerator CantMoveIntoWhenNotIsRevealed()
        {
            Card01115 parlor = _cardsProvider.GetCard<Card01115>();
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return AssertThatIsNotClickable(parlor);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE1.Hallway));
        }

        [UnityTest]
        public IEnumerator CanMoveIntoIfIsRevealed()
        {
            CardPlace Parlor = _cardsProvider.GetCard<Card01115>();
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<RevealGameAction>().SetWith(Parlor).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(Parlor);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.CurrentPlace, Is.EqualTo(Parlor));
        }
    }
}
