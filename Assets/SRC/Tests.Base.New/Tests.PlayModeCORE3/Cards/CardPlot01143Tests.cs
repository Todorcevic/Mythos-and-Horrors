using System.Collections;
using System.Linq;
using MythosAndHorrors.GameRules;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CardPlot01143Tests : TestCORE3Preparation
    {
        [UnityTest]
        public IEnumerator DrawMonsterWhenComplete()
        {
            Card01143 cardPlot = _cardsProvider.GetCard<Card01143>();
            yield return PlaceOnlyScene();
            //yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE3.Info.DangerCards.Take(10), SceneCORE3.DangerDiscardZone, isFaceDown: false)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            yield return _gameActionsProvider.Create(new CheckEldritchsPlotGameAction()).AsCoroutine();

            Assert.That(SceneCORE3.MainPath.OwnZone.Cards.Any(card => card is CardCreature mosnter && mosnter.HasThisTag(Tag.Monster)), Is.True);
        }
    }
}
