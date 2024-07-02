using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionChooseInvestigatorTest : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ChooseInvestigatorTest()
        {
            yield return StartingScene();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(_investigatorsProvider.Second.CurrentTurns, 1).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<ChooseInvestigatorGameAction>().SetWith().Execute();
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedTokenButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.Second.CurrentTurns.Value, Is.EqualTo(0));
        }
    }
}
