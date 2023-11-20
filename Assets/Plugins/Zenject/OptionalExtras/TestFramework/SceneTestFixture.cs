using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Zenject.Internal;
using Assert = ModestTree.Assert;

// Ignore warning about using SceneManager.UnloadScene instead of SceneManager.UnloadSceneAsync
#pragma warning disable 618

namespace Zenject
{
    public abstract class SceneTestFixture
    {
        private bool _hasLoadedScene;

        protected DiContainer SceneContainer { get; private set; }

        public IEnumerator LoadScene(string sceneName)
        {
            Assert.That(!_hasLoadedScene, "Attempted to load scene twice!");
            _hasLoadedScene = true;
            ZenjectTestUtil.DestroyEverythingExceptTestRunner(false);
            Assert.That(Application.CanStreamedLevelBeLoaded(sceneName),
                $"Cannot load scene {sceneName} for test {GetType()}. The scenes used by SceneTestFixture derived classes must be added to the build settings for the test to work");
            yield return RealLoadScene(sceneName);
            Assert.That(ProjectContext.HasInstance, $"{sceneName} has not ProjectContext");
            Scene scene = SceneManager.GetSceneByName(sceneName);
            SceneContainer = ProjectContext.Instance.Container.Resolve<SceneContextRegistry>()
                .TryGetSceneContextForScene(scene).Container;
            SceneContainer?.Inject(this);
        }

        private IEnumerator RealLoadScene(string sceneName)
        {
            AsyncOperation loader = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!loader.isDone) yield return null;
        }

        [UnitySetUp]
        public virtual IEnumerator SetUp()
        {
            SetMemberDefaults();
            yield return null;
        }

        void SetMemberDefaults()
        {
            StaticContext.Clear();
            _hasLoadedScene = false;
            SceneContainer = null;
        }

        [UnityTearDown]
        public virtual IEnumerator TearDown()
        {
            ZenjectTestUtil.DestroyEverythingExceptTestRunner(true);
            SetMemberDefaults();
            yield return null;
        }
    }
}
