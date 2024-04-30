using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01105Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator DiscardAll()
        {
            Card01105 cardPlot = _cardsProvider.GetCard<Card01105>();
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            Task<CheckEldritchsPlotGameAction> taskGameAction = _gameActionsProvider.Create(new CheckEldritchsPlotGameAction());
            if (!DEBUG_MODE) yield return WaitToCloneClick(0);
            while (!taskGameAction.IsCompleted) yield return null;
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count == 4), Is.True);
        }

        [UnityTest]
        public IEnumerator TakeDamage()
        {
            Card01105 cardPlot = _cardsProvider.GetCard<Card01105>();
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            Task<CheckEldritchsPlotGameAction> taskGameAction = _gameActionsProvider.Create(new CheckEldritchsPlotGameAction());
            if (!DEBUG_MODE) yield return WaitToCloneClick(1);
            while (!taskGameAction.IsCompleted) yield return null;
            Assert.That(_investigatorsProvider.Leader.Sanity.Value, Is.EqualTo(3));
        }
    }
}
