using Tuesday.GameView;
using UnityEngine;

namespace Tuesday.Tests
{
    public class PreAwakeExecution : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void ExecuteBeforeSceneLoad()
        {
            LoaderComponent loader = FindFirstObjectByType<LoaderComponent>();
            if (loader != null) loader.enabled = false;
        }
    }
}
