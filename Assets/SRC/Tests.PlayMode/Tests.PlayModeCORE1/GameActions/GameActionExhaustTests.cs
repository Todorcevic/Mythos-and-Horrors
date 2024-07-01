using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionExhaustTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator ExhaustTest()
        {
            Card cardToExhaust = _investigatorsProvider.First.FullDeck.First();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardToExhaust, _investigatorsProvider.First.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatesGameAction(cardToExhaust.Exausted, true)).AsCoroutine();

            Assert.That(cardToExhaust.Exausted.IsActive);

            yield return _gameActionsProvider.Create(new UpdateStatesGameAction(cardToExhaust.Exausted, false)).AsCoroutine();

            Assert.That(!cardToExhaust.Exausted.IsActive);
        }
    }
}
