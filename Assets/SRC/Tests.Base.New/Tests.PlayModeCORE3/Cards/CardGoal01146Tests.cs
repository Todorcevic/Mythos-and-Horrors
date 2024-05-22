using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{

    public class CardGoal01146Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CompleteEffect()
        {
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.DrewInterrogate, true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.VictoriaInterrogate, true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(SceneCORE3.CurrentGoal.Hints, 0)).AsCoroutine();

            Assert.That(SceneCORE3.Ritual.IsInPlay, Is.True);
            Assert.That(SceneCORE3.CultistsNotInterrogate().Count, Is.EqualTo(4));
            Assert.That(SceneCORE3.CultistsNotInterrogate().All(cultist => cultist.CurrentPlace == SceneCORE3.MainPath), Is.True);
        }
    }
}
