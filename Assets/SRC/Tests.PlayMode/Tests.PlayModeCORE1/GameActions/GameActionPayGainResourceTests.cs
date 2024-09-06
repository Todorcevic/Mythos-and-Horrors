using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    [TestFixture]
    public class GameActionPayGainResourceTests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Full_Take_Resource()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First, withResources: true);

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.First).Execute();
            yield return ClickedResourceButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.First.Resources.Value, Is.EqualTo(6));
        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Investigator()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First, withResources: true);

            yield return _gameActionsProvider.Create<GainResourceGameAction>().SetWith(_investigatorsProvider.First, 5).Execute().AsCoroutine();

            Assert.That(_investigatorsProvider.First.Resources.Value, Is.EqualTo(10));
        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Pile()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First, withResources: true);

            yield return _gameActionsProvider.Create<GainResourceGameAction>().SetWith(_investigatorsProvider.First, 5).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<PayResourceGameAction>().SetWith(_investigatorsProvider.First, 2).Execute().AsCoroutine();

            Assert.That(_investigatorsProvider.First.Resources.Value, Is.EqualTo(8));
        }
    }
}
