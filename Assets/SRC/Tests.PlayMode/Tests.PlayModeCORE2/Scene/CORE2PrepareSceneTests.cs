using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CORE2PrepareSceneTests : TestCORE2Preparation
    {
        [UnityTest]
        public IEnumerator PrepareSceneTest()
        {
            SceneCORE2 scene = SceneCORE2;
            yield return PlayAllInvestigators(withAvatar: false);

            yield return _gameActionsProvider.Create<PrepareSceneGameAction>().SetWith(scene).Start().AsCoroutine();

            Assert.That(scene.PlaceCards.Where(place => place.IsInPlay).Count(), Is.EqualTo(8));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
                Is.EqualTo(scene.Fluvial));
            Assert.That(scene.GoalZone.Cards.Unique(), Is.EqualTo(scene.FirstGoal));
            Assert.That(scene.PlotZone.Cards.Unique(), Is.EqualTo(scene.FirstPlot));
            Assert.That(scene.DangerDeckZone.Cards.Count(), Is.EqualTo(scene.StartDeckDangerCards.Count() - 3)); //-3 Acolics
            Assert.That(scene.Acolits.Count(acolic => acolic.IsInPlay), Is.EqualTo(3));
            Assert.That(scene.DangerDeckZone.Cards, Does.Not.Contains(scene.GhoulPriest));
        }

        [UnityTest]
        public IEnumerator PrepareSceneWithHouseUpAndGhoulLiveTest()
        {
            SceneCORE2 scene = SceneCORE2;
            yield return PlayAllInvestigators(withAvatar: false);
            yield return _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.HouseUp, true).Start();
            yield return _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.PriestGhoulLive, true).Start();
            yield return _gameActionsProvider.Create<PrepareSceneGameAction>().SetWith(scene).Start().AsCoroutine();

            Assert.That(scene.PlaceCards.Where(place => place.IsInPlay).Count(), Is.EqualTo(9));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
                Is.EqualTo(scene.Home));
            Assert.That(scene.DangerDeckZone.Cards, Contains.Item(scene.GhoulPriest));
        }
    }
}
