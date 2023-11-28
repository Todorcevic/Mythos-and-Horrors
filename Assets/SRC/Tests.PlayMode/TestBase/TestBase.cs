using MythsAndHorrors.GameView;
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
            InstallerToScene();
            yield return LoadScene("GamePlay", InstallerToTests);
        }

        private void InstallerToScene()
        {
            StaticContext.Container.BindInstance(false).WhenInjectedInto<InitializerComponent>();
            StaticContext.Container.Bind<FilesPath>().To<TestFilePath>().AsSingle();
        }

        private void InstallerToTests()
        {
            SceneContainer.Bind<CardBuilder>().AsSingle();
            SceneContainer.Bind<CardViewBuilder>().AsSingle();
        }

        protected IEnumerator PressAnyKey() => new WaitUntil(() => Input.anyKeyDown);
    }
}
