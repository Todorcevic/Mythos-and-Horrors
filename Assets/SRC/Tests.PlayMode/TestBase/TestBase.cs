using MythsAndHorrors.GameView;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    public class TestBase : SceneTestFixture
    {
        protected virtual bool DEBUG_MODE => false;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            if (!DEBUG_MODE) WithoutAnimations();
            InstallerToScene();
            yield return LoadScene("GamePlay", InstallerToTests);
        }

        [UnityTearDown]
        public override IEnumerator TearDown()
        {
            yield return WaitLoadImages();
            SetTimeDefault();
            yield return base.TearDown();
        }

        protected void WithoutAnimations()
        {
            ViewValues.FAST_TIME_ANIMATION = 0f;
            ViewValues.DEFAULT_TIME_ANIMATION = 0f;
            ViewValues.MID_TIME_ANIMATION = 0f;
            ViewValues.SLOW_TIME_ANIMATION = 0f;
        }

        private void SetTimeDefault()
        {
            ViewValues.FAST_TIME_ANIMATION = 0.25f;
            ViewValues.DEFAULT_TIME_ANIMATION = 0.4f;
            ViewValues.MID_TIME_ANIMATION = 0.5f;
            ViewValues.SLOW_TIME_ANIMATION = 0.75f;
        }

        private void InstallerToScene()
        {
            StaticContext.Container.BindInstance(false).WhenInjectedInto<InitializerComponent>();
            StaticContext.Container.Bind<FilesPath>().To<TestFilePath>().AsSingle();
        }

        private void InstallerToTests()
        {
            SceneContainer.Bind<CardInfoBuilder>().AsTransient();
            SceneContainer.Bind<CardBuilder>().AsSingle();
            SceneContainer.Bind<CardViewBuilder>().AsSingle();
        }

        protected IEnumerator PressAnyKey() => new WaitUntil(() => Input.anyKeyDown);

        protected IEnumerator WaitLoadImages() => new WaitUntil(ImageExtension.IsAllDone);
    }
}
