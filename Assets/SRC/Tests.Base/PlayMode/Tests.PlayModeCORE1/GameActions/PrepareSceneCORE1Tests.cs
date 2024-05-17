using MythosAndHorrors.GameRules;
using System.Linq;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class PrepareSceneCORE1Tests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PrepareSceneTest()
        {
            SceneCORE1 scene = (SceneCORE1)_chaptersProvider.CurrentScene;
            yield return _preparationSceneCORE1.PlayAllInvestigators(withAvatar: false);

            yield return _gameActionsProvider.Create(new PrepareSceneGameAction(scene)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Count(), Is.EqualTo(4));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
                Is.EqualTo(scene.Study));
            Assert.That(scene.GoalZone.Cards.Unique(), Is.EqualTo(scene.FirstGoal));
            Assert.That(scene.PlotZone.Cards.Unique(), Is.EqualTo(scene.FirstPlot));
            Assert.That(scene.DangerDeckZone.Cards.Count(), Is.EqualTo(scene.StartDangerCards.Count()));
        }
    }
}
