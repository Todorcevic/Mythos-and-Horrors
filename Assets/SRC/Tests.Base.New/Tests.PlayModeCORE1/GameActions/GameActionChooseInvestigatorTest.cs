using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class GameActionChooseInvestigatorTest : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator ChooseInvestigatorTest()
        {
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.Second.CurrentTurns, 1)).AsCoroutine();

            Task<ChooseInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new ChooseInvestigatorGameAction());
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedTokenButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.Second.CurrentTurns.Value, Is.EqualTo(0));
        }
    }
}
