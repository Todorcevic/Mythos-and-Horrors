using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardGoal01146Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CompleteEffect()
        {
            yield return StartingScene();
            yield return _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.DrewInterrogate, true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.VictoriaInterrogate, true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(SceneCORE3.CurrentGoal.Hints, 0).Execute().AsCoroutine();

            Assert.That(SceneCORE3.Ritual.IsInPlay.IsTrue, Is.True);
            Assert.That(SceneCORE3.CultistsNotInterrogate().Count, Is.EqualTo(4));
            Assert.That(SceneCORE3.CultistsNotInterrogate().All(cultist => cultist.CurrentPlace == SceneCORE3.MainPath), Is.True);
        }
    }
}
