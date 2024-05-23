using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using MythosAndHorrors.PlayMode.Tests;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class ShuffleTests : PlayModeTestsBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Shuffle_Zone()
        {
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.FullDeck, _investigatorsProvider.First.DeckZone)).AsCoroutine();
            CardView[] allCardViews = _zoneViewsManager.Get(_investigatorsProvider.First.DeckZone).GetComponentsInChildren<CardView>();
            do
            {
                yield return _gameActionsProvider.Create(new ShuffleGameAction(_investigatorsProvider.First.DeckZone)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);
            CardView[] allCardViewsShuffled = _zoneViewsManager.Get(_investigatorsProvider.First.DeckZone).GetComponentsInChildren<CardView>();
            Assert.That(!allCardViews.SequenceEqual(allCardViewsShuffled));
        }
    }
}
