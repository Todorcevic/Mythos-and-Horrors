using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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
            yield return _preparationScene.PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new InitialDrawGameAction(_investigatorsProvider.Leader)).AsCoroutine();
            yield return _gameActionsProvider.Create(new InitialDrawGameAction(_investigatorsProvider.Second)).AsCoroutine();
            yield return _gameActionsProvider.Create(new InitialDrawGameAction(_investigatorsProvider.Third)).AsCoroutine();
            yield return _gameActionsProvider.Create(new InitialDrawGameAction(_investigatorsProvider.Fourth)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            Task<CheckEldritchsPlotGameAction> taskGameAction = _gameActionsProvider.Create(new CheckEldritchsPlotGameAction());
            while (_gameActionsProvider.CurrentInteractable == null) yield return null;
            List<Effect> allEffects = _gameActionsProvider.CurrentInteractable.GetPrivateMember<List<Effect>>("_allCardEffects");
            if (!DEBUG_MODE) yield return WaitToCloneClick(allEffects[0]);
            while (!taskGameAction.IsCompleted) yield return null;

            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count == 4), Is.True);
        }

        [UnityTest]
        public IEnumerator TakeDamage()
        {
            Card01105 cardPlot = _cardsProvider.GetCard<Card01105>();
            yield return _preparationScene.PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            Task<CheckEldritchsPlotGameAction> taskGameAction = _gameActionsProvider.Create(new CheckEldritchsPlotGameAction());
            while (_gameActionsProvider.CurrentInteractable == null) yield return null;
            List<Effect> allEffects = _gameActionsProvider.CurrentInteractable.GetPrivateMember<List<Effect>>("_allCardEffects");
            if (!DEBUG_MODE) yield return WaitToCloneClick(allEffects[1]);
            while (!taskGameAction.IsCompleted) yield return null;

            Assert.That(_investigatorsProvider.Leader.Sanity.Value, Is.EqualTo(3));
        }
    }
}
