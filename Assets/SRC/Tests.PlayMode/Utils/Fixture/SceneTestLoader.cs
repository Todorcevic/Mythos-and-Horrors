using System;
using Zenject;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject.Internal;
using NUnit.Framework;
using UnityEngine.TestTools;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;

namespace MythosAndHorrors.PlayMode.Tests
{
    public abstract class SceneTestLoader
    {
        private static string currentFileLoaded;
        private static string currentSceneName;
        private static TestsType currentTestType;
        private bool _hasLoadedScene;

        protected static DiContainer SceneContainer { get; set; }
        protected abstract string JSON_SAVE_DATA_PATH { get; }
        protected abstract string SCENE_NAME { get; }
        protected abstract TestsType TestsType { get; }
        private bool IsDifferentScene =>
            currentFileLoaded != JSON_SAVE_DATA_PATH || currentSceneName != SCENE_NAME || currentTestType != TestsType;

        /*******************************************************************/
        [UnitySetUp]
        public virtual IEnumerator SetUp()
        {
            if (IsDifferentScene)
            {
                SaveState();
                if (TestsType == TestsType.Unit) PrepareUnitTests();
                else yield return PrepareIntegrationTests();
            }
            else SceneContainer?.Inject(this);

            void SaveState()
            {
                currentFileLoaded = JSON_SAVE_DATA_PATH;
                currentSceneName = SCENE_NAME;
                currentTestType = TestsType;
            }
        }

        protected virtual void PrepareUnitTests()
        {
            SceneContainer = new();
            SceneContainer.Install<InjectionService>();
            InstallerToSceneInUnitMode();
            SceneContainer?.Inject(this);

            void InstallerToSceneInUnitMode()
            {
                SceneContainer.BindInstance(JSON_SAVE_DATA_PATH).WhenInjectedInto<DataSaveUseCase>();
                StaticContext.Container.BindInstance(false).WhenInjectedInto<InitializerComponent>();//Only for PlayModeViewTests
                InstallFakes();
            }

            void InstallFakes()
            {
                SceneContainer.Rebind<IPresenterInteractable>().To<InteractableFake>().AsSingle();
                SceneContainer.Rebind<IPresenterAnimation>().To<PresenterFake>().AsSingle();
            }
        }

        protected virtual IEnumerator PrepareIntegrationTests()
        {
            ClearContainer();
            InstallerToSceneIntegrationMode();
            yield return LoadScene(SCENE_NAME);
            SceneContainer?.Inject(this);

            void ClearContainer()
            {
                ZenjectTestUtil.DestroyEverythingExceptTestRunner(false);
                StaticContext.Clear();
                _hasLoadedScene = false;
                SceneContainer = null;
            }

            void InstallerToSceneIntegrationMode()
            {
                StaticContext.Container.BindInstance(JSON_SAVE_DATA_PATH).WhenInjectedInto<DataSaveUseCase>();
                StaticContext.Container.BindInstance(false).WhenInjectedInto<InitializerComponent>();
            }
        }

        private IEnumerator LoadScene(string unitySceneName)
        {
            Assert.That(!_hasLoadedScene, "Attempted to load scene twice!");
            _hasLoadedScene = true;
            Assert.That(Application.CanStreamedLevelBeLoaded(unitySceneName), $"Cannot load scene {unitySceneName} for test {GetType()}. The scenes used by SceneTestFixture derived classes must be added to the build settings for the test to work");
            yield return RealLoadScene(unitySceneName);
            Assert.That(ProjectContext.HasInstance, $"{unitySceneName} has not ProjectContext");
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetSceneByName(unitySceneName);
            SceneContainer = ProjectContext.Instance.Container.Resolve<SceneContextRegistry>().TryGetSceneContextForScene(scene).Container;

            static IEnumerator RealLoadScene(string sceneName)
            {
                AsyncOperation loader = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
                while (!loader.isDone) yield return null;
            }
        }
    }
}