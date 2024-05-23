using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardPlot01121aTests : TestCORE2Preparation
    {
        [UnityTest]
        public IEnumerator WhenRevealHunterSpawn()
        {
            Card01121a plot = _cardsProvider.GetCard<Card01121a>();

            yield return StartingScene();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(plot.Eldritch, 6)).AsCoroutine();
            yield return _gameActionsProvider.Create(new CheckEldritchsPlotGameAction()).AsCoroutine();

            Assert.That(plot.CurrentZone, Is.EqualTo(SceneCORE2.OutZone));
            Assert.That(SceneCORE2.MaskedHunter.IsInPlay, Is.True);
        }
    }
}
