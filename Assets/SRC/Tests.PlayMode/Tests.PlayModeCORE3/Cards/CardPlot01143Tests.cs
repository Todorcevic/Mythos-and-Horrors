using System.Collections;
using System.Linq;
using MythosAndHorrors.GameRules;
using NUnit.Framework;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardPlot01143Tests : TestCORE3Preparation
    {
        [UnityTest]
        public IEnumerator DrawMonsterWhenComplete()
        {
            Card01143 cardPlot = _cardsProvider.GetCard<Card01143>();
            yield return PlaceOnlyScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.DangerCards.Take(10), SceneCORE3.DangerDiscardZone, isFaceDown: false).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardPlot.Eldritch, cardPlot.Eldritch.Value).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<CheckEldritchsPlotGameAction>().Execute().AsCoroutine();

            CardCreature monster = SceneCORE3.MainPath.OwnZone.Cards.OfType<CardCreature>().First();
            Assert.That(monster.Eldritch.Value, Is.EqualTo(1));
        }
    }
}
