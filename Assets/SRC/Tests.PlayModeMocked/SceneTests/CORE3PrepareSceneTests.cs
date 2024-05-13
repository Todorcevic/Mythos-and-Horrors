using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Linq;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CORE3PrepareSceneTests : TestCORE1PlayModeBase
    {
        [UnityTest]
        public IEnumerator PrepareSceneTest()
        {
            SceneCORE1 scene = _preparationSceneCORE1.SceneCORE1;
            yield return _preparationSceneCORE1.PlayAllInvestigators(withAvatar: false).AsCoroutine();

            yield return _gameActionsProvider.Create(new PrepareSceneGameAction(scene)).AsCoroutine();

            Assert.That(scene.Info.PlaceCards.Where(place => place.IsInPlay).Count(), Is.EqualTo(1));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
                Is.EqualTo(scene.Study));
        }
    }
}
