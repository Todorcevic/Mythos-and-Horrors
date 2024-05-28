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
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(adversityCard, investigator.DangerZone)).AsCoroutine();
            Assert.That(adversityCard.Hints.Value, Is.EqualTo(3));
            yield return _gameActionsProvider.Create(new FinalizeGameAction(SceneCORE1.FullResolutions[0])).AsCoroutine();
            Assert.That(investigator.Shock.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator NoTakeShock()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            Card01507 adversityCard = _cardsProvider.GetCard<Card01507>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(adversityCard, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(adversityCard.Hints, 0)).AsCoroutine();

            Assert.That(adversityCard.Hints.Value, Is.EqualTo(0));
            yield return _gameActionsProvider.Create(new FinalizeGameAction(SceneCORE1.FullResolutions[0])).AsCoroutine();
            Assert.That(investigator.Shock.Value, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator PayHint()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return StartingScene();
            int investigatorPlaceHintsExpected = investigator.CurrentPlace.Hints.Value - 1;
            Card01507 adversityCard = _cardsProvider.GetCard<Card01507>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(adversityCard, investigator.DangerZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
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
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ghoul, place.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(noGhoul, place.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardPlot.AmountOfEldritch, Is.EqualTo(1));
        }
    }
}
