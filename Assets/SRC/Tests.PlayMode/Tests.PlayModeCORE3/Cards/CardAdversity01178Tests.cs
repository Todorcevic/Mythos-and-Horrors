using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Linq;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardAdversity01178Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ChooseDrawCards()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01178 cardAdversity = _cardsProvider.GetCard<Card01178>();

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            Task taskGameAction = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute();
            int currentDeckSize = investigator.DeckZone.Cards.Count;
            yield return ClickedClone(cardAdversity, 0, isReaction: true);
            yield return taskGameAction.AsCoroutine();

            Assert.That(SceneCORE3.CurrentPlot.AmountOfEldritch, Is.EqualTo(2));
            Assert.That(investigator.DeckZone.Cards.Count, Is.EqualTo(currentDeckSize - 2));
        }

        [UnityTest]
        public IEnumerator ChooseTakeFear()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01178 cardAdversity = _cardsProvider.GetCard<Card01178>();

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            Task taskGameAction = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute();
            yield return ClickedClone(cardAdversity, 1, isReaction: true);
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator Peril()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01178 cardAdversity = _cardsProvider.GetCard<Card01178>();
            CardPlot plot = SceneCORE3.PlotCards.ElementAt(1);
            yield return StartingScene();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.CurrentPlot, SceneCORE3.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(plot, SceneCORE3.PlotZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(plot.Eldritch, 4).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute();
            yield return ClickedClone(cardAdversity, 0, isReaction: true);
            Assert.That(investigator.Isolated.IsActive, Is.True);
            Assert.That(_gameActionsProvider.CurrentInteractable.AllPlayableEffects.All(cardEffect => cardEffect.Investigator == investigator), Is.True);

            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Isolated.IsActive, Is.False);
        }
    }
}
