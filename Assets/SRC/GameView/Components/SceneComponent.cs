using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SceneComponent : MonoBehaviour
    {
        private const string FIRST_SCENE = "GamePlay";

        [Inject] readonly ZenjectSceneLoader _sceneLoader;

        public void Start()
        {
            StartCoroutine(AdvanceScene());
        }

        public IEnumerator AdvanceScene()
        {
            AsyncOperation asyncOperation = _sceneLoader.LoadSceneAsync(FIRST_SCENE, LoadSceneMode.Single, (container) =>
            {
                container.BindInstance(true).WhenInjectedInto<LoaderComponent>();
            });

            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }
    }
}
