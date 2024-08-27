using MythosAndHorrors.GameRules;
using System.Linq;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CORE1PrepareSceneTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator PrepareSceneTest()
        {
            SceneCORE1 scene = SceneCORE1;
            yield return PlayAllInvestigators(withAvatar: false);

            yield return _gameActionsProvider.Create<PrepareSceneGameAction>().SetWith(scene).Execute().AsCoroutine();

            Assert.That(scene.PlaceCards.Where(place => place.IsInPlay.IsTrue).Count(), Is.EqualTo(1));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
                Is.EqualTo(scene.Study));
            Assert.That(scene.GoalZone.Cards.Unique(), Is.EqualTo(scene.FirstGoal));
            Assert.That(scene.PlotZone.Cards.Unique(), Is.EqualTo(scene.FirstPlot));
            Assert.That(scene.DangerDeckZone.Cards.Count(), Is.EqualTo(scene.StartDeckDangerCards.Count()));
        }
    }
}
