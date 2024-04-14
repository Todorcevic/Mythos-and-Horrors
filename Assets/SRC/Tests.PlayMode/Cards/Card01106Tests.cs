using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01106Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator DrawGhoul()
        {
            Card01106 cardPlot = _cardsProvider.GetCard<Card01106>();
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.First);
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
