using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Linq;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CORE3PrepareSceneTests : TestCORE3Preparation
    {
        [UnityTest]
        public IEnumerator PrepareSceneTest()
        {
            SceneCORE3 scene = SceneCORE3;
            yield return PlayAllInvestigators(withAvatar: false);

            yield return _gameActionsProvider.Create(new PrepareSceneGameAction(scene)).AsCoroutine();

            Assert.That(scene.Info.PlaceCards.Where(place => place.IsInPlay).Count(), Is.EqualTo(5));
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
            yield return _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.PriestGhoulLive, true));
            yield return _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.DrewInterrogate, true));
            yield return _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.MaskedHunterInterrogate, true));
            yield return _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.PeterInterrogate, true));
            yield return _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.IsMidknigh, true));

            yield return _gameActionsProvider.Create(new PrepareSceneGameAction(scene)).AsCoroutine();

            Assert.That(scene.FirstPlot.AmountOfEldritch, Is.EqualTo(2));
            Assert.That(scene.DangerDeckZone.Cards, Contains.Item(scene.GhoulPriest));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandSize == 3), Is.True);
        }
    }
}
