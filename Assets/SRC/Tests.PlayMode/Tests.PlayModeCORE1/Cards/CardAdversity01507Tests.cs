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
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversityCard, investigator.DangerZone).Execute().AsCoroutine();
            Assert.That(adversityCard.Keys.Value, Is.EqualTo(3));
            yield return _gameActionsProvider.Create<FinalizeGameAction>().SetWith(SceneCORE1.FullResolutions[0]).Execute().AsCoroutine();
            Assert.That(investigator.Shock.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator NoTakeShock()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            Card01507 adversityCard = _cardsProvider.GetCard<Card01507>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversityCard, investigator.DangerZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(adversityCard.Keys, 0).Execute().AsCoroutine();

            Assert.That(adversityCard.Keys.Value, Is.EqualTo(0));
            yield return _gameActionsProvider.Create<FinalizeGameAction>().SetWith(SceneCORE1.FullResolutions[0]).Execute().AsCoroutine();
            Assert.That(investigator.Shock.Value, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator PayKey()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return StartingScene();
            int investigatorPlaceKeysExpected = investigator.CurrentPlace.Keys.Value;
            Card01507 adversityCard = _cardsProvider.GetCard<Card01507>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversityCard, investigator.DangerZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(adversityCard);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(adversityCard.Keys.Value, Is.EqualTo(2));
            Assert.That(investigator.Keys.Value, Is.EqualTo(0));
            Assert.That(investigator.CurrentPlace.Keys.Value, Is.EqualTo(investigatorPlaceKeysExpected));
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
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(ghoul, place.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(noGhoul, place.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlot, _chaptersProvider.CurrentScene.PlotZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<RoundGameAction>().Execute();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardPlot.AmountDecrementedEldritch, Is.EqualTo(1));
        }
    }
}
