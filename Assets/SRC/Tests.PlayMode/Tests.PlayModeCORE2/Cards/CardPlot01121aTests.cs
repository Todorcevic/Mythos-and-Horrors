using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardPlot01121aTests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator WhenRevealHunterSpawn()
        {
            Card01120 plot = _cardsProvider.GetCard<Card01120>();

            yield return StartingScene();
            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(plot.Eldritch, 6).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<CheckEldritchsPlotGameAction>().Execute().AsCoroutine();

            Assert.That(plot.CurrentZone, Is.EqualTo(SceneCORE2.OutZone));
            Assert.That(SceneCORE2.MaskedHunter.IsInPlay.IsTrue, Is.True);
        }
    }
}
