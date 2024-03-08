using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class ShuffleGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUse;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ZoneViewsManager zoneViewsManager;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Shuffle_Zone()
        {
            _prepareGameUse.Execute();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.FullDeck, _investigatorsProvider.Leader.DeckZone)).AsCoroutine();
            CardView[] allCardViews = zoneViewsManager.Get(_investigatorsProvider.Leader.DeckZone).GetComponentsInChildren<CardView>();
            do
            {
                yield return _gameActionFactory.Create(new ShuffleGameAction(_investigatorsProvider.Leader.DeckZone)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);
            CardView[] allCardViewsShuffled = zoneViewsManager.Get(_investigatorsProvider.Leader.DeckZone).GetComponentsInChildren<CardView>();
            Assert.That(!allCardViews.SequenceEqual(allCardViewsShuffled));
        }
    }
}
