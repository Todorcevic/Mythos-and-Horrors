using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01105Tests : TestBase
    {
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator DiscardAll()
        {
            Card01105 cardPlot = (Card01105)_cardsProvider.GetCard<CardPlot>("01105");
            yield return PlayAllInvestigators();

            yield return _gameActionsProvider.Create(new InitialDrawGameAction(_investigatorProvider.Leader)).AsCoroutine();
            yield return _gameActionsProvider.Create(new InitialDrawGameAction(_investigatorProvider.Second)).AsCoroutine();
            yield return _gameActionsProvider.Create(new InitialDrawGameAction(_investigatorProvider.Third)).AsCoroutine();
            yield return _gameActionsProvider.Create(new InitialDrawGameAction(_investigatorProvider.Fourth)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            Task<CheckEldritchsPlotGameAction> taskGameAction = _gameActionsProvider.Create(new CheckEldritchsPlotGameAction());
            while (cardPlot.DiscardAllInvestigatorsEffect == null) yield return null;
            if (!DEBUG_MODE) yield return WaitToCloneClick(cardPlot.DiscardAllInvestigatorsEffect);
            while (!taskGameAction.IsCompleted) yield return null;

            Assert.That(_investigatorProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count() == 4), Is.True);
        }

        [UnityTest]
        public IEnumerator TakeDamage()
        {
            Card01105 cardPlot = (Card01105)_cardsProvider.GetCard<CardPlot>("01105");
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            Task<CheckEldritchsPlotGameAction> taskGameAction = _gameActionsProvider.Create(new CheckEldritchsPlotGameAction());
            while (cardPlot.DamageEffect == null) yield return null;
            if (!DEBUG_MODE) yield return WaitToCloneClick(cardPlot.DamageEffect);
            while (!taskGameAction.IsCompleted) yield return null;

            Assert.That(_investigatorProvider.Leader.Sanity.Value, Is.EqualTo(3));
        }
    }
}
