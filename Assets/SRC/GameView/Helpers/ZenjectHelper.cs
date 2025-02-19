using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public static class ZenjectHelper
    {
        private static DiContainer _container;

        /*******************************************************************/
        public static void Initialize(DiContainer container)
        {
            _container = container;
        }

        /*******************************************************************/
        public static T Instantiate<T>(T original, Transform parent) where T : Component
        {
            return _container.InstantiatePrefabForComponent<T>(original, parent);
        }
    }
}
