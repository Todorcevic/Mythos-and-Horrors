using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardPlot01105Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator SelectAllInvestigatorDiscardOneCard()
        {
            Card01105 cardPlot = _cardsProvider.GetCard<Card01105>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            Task<CheckEldritchsPlotGameAction> taskGameAction = _gameActionsProvider.Create(new CheckEldritchsPlotGameAction());
            yield return ClickedClone(cardPlot, 0, isReaction: true);
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count == 4), Is.True);
        }

        [UnityTest]
        public IEnumerator SelectLeadInvestigatorTakeFear()
        {
            Card01105 cardPlot = _cardsProvider.GetCard<Card01105>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            Task<CheckEldritchsPlotGameAction> taskGameAction = _gameActionsProvider.Create(new CheckEldritchsPlotGameAction());
            yield return ClickedClone(cardPlot, 1, isReaction: true);
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Leader.FearRecived.Value, Is.EqualTo(2));
        }
    }
}
