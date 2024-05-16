using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CORE2PrepareSceneTests : TestCORE2PlayModeBase
    {
        [UnityTest]
        public IEnumerator PrepareSceneTest()
        {
            SceneCORE2 scene = _preparationSceneCORE2.SceneCORE2;
            yield return _preparationSceneCORE2.PlayAllInvestigators(withAvatar: false);

            yield return _gameActionsProvider.Create(new PrepareSceneGameAction(scene)).AsCoroutine();

            Assert.That(scene.Info.PlaceCards.Where(place => place.IsInPlay).Count(), Is.EqualTo(8));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
                Is.EqualTo(scene.Fluvial));
            Assert.That(scene.GoalZone.Cards.Unique(), Is.EqualTo(scene.FirstGoal));
            Assert.That(scene.PlotZone.Cards.Unique(), Is.EqualTo(scene.FirstPlot));
            Assert.That(scene.DangerDeckZone.Cards.Count(), Is.EqualTo(scene.StartDangerCards.Count() - 3)); //-3 Acolics
            Assert.That(scene.Acolits.Count(acolic => acolic.IsInPlay), Is.EqualTo(3));
            Assert.That(scene.DangerDeckZone.Cards, Does.Not.Contains(scene.GhoulPriest));
        }

        [UnityTest]
        public IEnumerator PrepareSceneWithHouseUpAndGhoulLiveTest()
        {
            SceneCORE2 scene = _preparationSceneCORE2.SceneCORE2;
            yield return _preparationSceneCORE2.PlayAllInvestigators(withAvatar: false);
            yield return _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.HouseUp, true));
            yield return _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.PriestGhoulLive, true));
            yield return _gameActionsProvider.Create(new PrepareSceneGameAction(scene)).AsCoroutine();

            Assert.That(scene.Info.PlaceCards.Where(place => place.IsInPlay).Count(), Is.EqualTo(9));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
                Is.EqualTo(scene.Home));
            Assert.That(scene.DangerDeckZone.Cards, Contains.Item(scene.GhoulPriest));
        }
    }
}
