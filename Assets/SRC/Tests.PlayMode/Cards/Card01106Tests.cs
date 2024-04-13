using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01106Tests : TestBase
    {
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator DrawGhoul()
        {
            Card01106 cardPlot = (Card01106)_cardsProvider.GetCard<CardPlot>("01106");
            yield return PlayThisInvestigator(_investigatorProvider.First);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_chaptersProvider.CurrentScene.Info.DangerCards, _chaptersProvider.CurrentScene.DangerDeckZone, isFaceDown: true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_chaptersProvider.CurrentScene.Info.DangerCards.Take(10), _chaptersProvider.CurrentScene.DangerDiscardZone, isFaceDown: false)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new CheckEldritchsPlotGameAction()).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_chaptersProvider.CurrentScene.Info.DangerCards.Any(card => card is IGhoul && card.IsInPlay), Is.True);
        }
    }
}
