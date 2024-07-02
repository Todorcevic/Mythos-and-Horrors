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

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardToExhaust, _investigatorsProvider.First.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(cardToExhaust.Exausted, true).Execute().AsCoroutine();

            Assert.That(cardToExhaust.Exausted.IsActive);

            yield return _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(cardToExhaust.Exausted, false).Execute().AsCoroutine();

            Assert.That(!cardToExhaust.Exausted.IsActive);
        }
    }
}
