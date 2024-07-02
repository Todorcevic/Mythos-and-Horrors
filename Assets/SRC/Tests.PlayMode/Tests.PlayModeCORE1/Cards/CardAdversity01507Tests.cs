using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardAdversity01507Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator TakeShock()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            Card01507 adversityCard = _cardsProvider.GetCard<Card01507>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversityCard, investigator.DangerZone).Start().AsCoroutine();
            Assert.That(adversityCard.Hints.Value, Is.EqualTo(3));
            yield return _gameActionsProvider.Create<FinalizeGameAction>().SetWith(SceneCORE1.FullResolutions[0]).Start().AsCoroutine();
            Assert.That(investigator.Shock.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator NoTakeShock()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            Card01507 adversityCard = _cardsProvider.GetCard<Card01507>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversityCard, investigator.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(adversityCard.Hints, 0).Start().AsCoroutine();

            Assert.That(adversityCard.Hints.Value, Is.EqualTo(0));
            yield return _gameActionsProvider.Create<FinalizeGameAction>().SetWith(SceneCORE1.FullResolutions[0]).Start().AsCoroutine();
            Assert.That(investigator.Shock.Value, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator PayHint()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return StartingScene();
            int investigatorPlaceHintsExpected = investigator.CurrentPlace.Hints.Value;
            Card01507 adversityCard = _cardsProvider.GetCard<Card01507>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversityCard, investigator.DangerZone).Start().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(adversityCard);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(adversityCard.Hints.Value, Is.EqualTo(2));
            Assert.That(investigator.Hints.Value, Is.EqualTo(0));
            Assert.That(investigator.CurrentPlace.Hints.Value, Is.EqualTo(investigatorPlaceHintsExpected));
        }

        [UnityTest]
        public IEnumerator RareBugFixed()
        {
            CardPlace place = _cardsProvider.GetCard<Card01112>();
            CardCreature ghoul = _cardsProvider.GetCard<Card01119>();
            CardCreature noGhoul = _cardsProvider.GetCard<Card01603>();
            CardPlot cardPlot = _cardsProvider.GetCard<Card01107>();
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(ghoul, place.OwnZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(noGhoul, place.OwnZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlot, _chaptersProvider.CurrentScene.PlotZone).Start().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardPlot.AmountOfEldritch, Is.EqualTo(1));
        }
    }
}
