using MythosAndHorrors.GameRules;
using System.Linq;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class PrepareSceneCORE2Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PrepareSceneTest()
        {
            SceneCORE2 scene = (SceneCORE2)_chaptersProvider.CurrentScene;
            yield return _preparationScene.PlayAllInvestigators(withAvatar: false);

            yield return _gameActionsProvider.Create(new PrepareSceneGameAction(scene)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(scene.Info.PlaceCards.Where(place => place.IsInPlay).Count(), Is.EqualTo(9));
            Assert.That(scene.Acolits.Where(place => place.IsInPlay).Count(), Is.EqualTo(3));
            Assert.That(scene.DangerDeckZone.Cards.Count(), Is.EqualTo(scene.RealDangerCards.Count()));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
           Is.EqualTo(scene.Fluvial));
        }
    }
}
