using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class GameActionPayGainResourceTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator Full_Take_Resource()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First, withResources: true);

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            yield return ClickedTokenButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.First.Resources.Value, Is.EqualTo(6));
        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Investigator()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First, withResources: true);

            yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.First, 5)).AsCoroutine();

            Assert.That(_investigatorsProvider.First.Resources.Value, Is.EqualTo(10));
        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Pile()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First, withResources: true);

            yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.First, 5)).AsCoroutine();
            yield return _gameActionsProvider.Create(new PayResourceGameAction(_investigatorsProvider.First, 2)).AsCoroutine();

            Assert.That(_investigatorsProvider.First.Resources.Value, Is.EqualTo(8));
        }
    }
}
