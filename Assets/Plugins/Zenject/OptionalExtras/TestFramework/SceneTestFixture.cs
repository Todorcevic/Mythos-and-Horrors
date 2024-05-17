using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject.Internal;
using Assert = ModestTree.Assert;

// Ignore warning about using SceneManager.UnloadScene instead of SceneManager.UnloadSceneAsync
#pragma warning disable 618

namespace Zenject
{
    public abstract class SceneTestFixture
    {
        private bool _hasLoadedScene;

        protected static DiContainer SceneContainer { get; set; }

        public IEnumerator LoadScene(string unitySceneName, Action actionInstaller = null)
        {
            Assert.That(!_hasLoadedScene, "Attempted to load scene twice!");
            _hasLoadedScene = true;
            ZenjectTestUtil.DestroyEverythingExceptTestRunner(false);
            Assert.That(Application.CanStreamedLevelBeLoaded(unitySceneName),
                $"Cannot load scene {unitySceneName} for test {GetType()}. The scenes used by SceneTestFixture derived classes must be added to the build settings for the test to work");
            yield return RealLoadScene(unitySceneName);
            Assert.That(ProjectContext.HasInstance, $"{unitySceneName} has not ProjectContext");
            Scene scene = SceneManager.GetSceneByName(unitySceneName);
            SceneContainer = ProjectContext.Instance.Container.Resolve<SceneContextRegistry>()
                .TryGetSceneContextForScene(scene).Container;
            actionInstaller?.Invoke();
            SceneContainer?.Inject(this);
        }

        private IEnumerator RealLoadScene(string sceneName)
        {
            AsyncOperation loader = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!loader.isDone) yield return null;
        }

        public virtual void ClearContainer()
        {
            ZenjectTestUtil.DestroyEverythingExceptTestRunner(true);
            StaticContext.Clear();
            _hasLoadedScene = false;
            SceneContainer = null;
        }
    }
}
