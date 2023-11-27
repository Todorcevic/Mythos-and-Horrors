using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    public class TestBase : SceneTestFixture
    {
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            StaticContext.Container.BindInstance(false).WhenInjectedInto<InitializerComponent>();
            yield return LoadScene("GamePlay", TestInstaller);
        }

        private void TestInstaller()
        {
            SceneContainer.Bind<CardBuilder>().AsSingle();
            SceneContainer.Bind<CardViewBuilder>().AsSingle();
        }

        protected IEnumerator PressAnyKey() => new WaitUntil(() => Input.anyKeyDown);
    }
}
