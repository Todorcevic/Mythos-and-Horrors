using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Linq;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CORE3PrepareSceneTests : TestCORE3Preparation
    {
        [UnityTest]
        public IEnumerator PrepareSceneTest()
        {
            SceneCORE3 scene = SceneCORE3;
            yield return PlayAllInvestigators(withAvatar: false);

            yield return _gameActionsProvider.Create<PrepareSceneGameAction>().SetWith(scene).Execute().AsCoroutine();

            Assert.That(scene.PlaceCards.Where(place => place.IsInPlay.IsTrue).Count(), Is.EqualTo(5));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
                Is.EqualTo(scene.MainPath));
            Assert.That(scene.GoalZone.Cards.Unique(), Is.EqualTo(scene.FirstGoal));
            Assert.That(scene.PlotZone.Cards.Unique(), Is.EqualTo(scene.FirstPlot));
            Assert.That(scene.DangerDeckZone.Cards.Count(), Is.EqualTo(scene.StartDeckDangerCards.Count()));
            Assert.That(scene.DangerDeckZone.Cards, Does.Not.Contains(scene.GhoulPriest));
        }

        [UnityTest]
        public IEnumerator PrepareSceneTest2()
        {
            SceneCORE3 scene = SceneCORE3;
            yield return PlayAllInvestigators(withAvatar: false);
            yield return _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.PriestGhoulLive, true).Execute();
            yield return _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.DrewInterrogate, true).Execute();
            yield return _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.MaskedHunterInterrogate, true).Execute();
            yield return _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.PeterInterrogate, true).Execute();
            yield return _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.IsMidknigh, true).Execute();

            yield return _gameActionsProvider.Create<PrepareSceneGameAction>().SetWith(scene).Execute().AsCoroutine();

            Assert.That(scene.FirstPlot.AmountOfEldritch, Is.EqualTo(2));
            Assert.That(scene.DangerDeckZone.Cards, Contains.Item(scene.GhoulPriest));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandSize == 3), Is.True);
        }
    }
}
