using MythsAndHorrors.GameRules;
using System;
using System.Reflection;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SceneLoaderUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly DiContainer _diContainer;

        /*******************************************************************/
        public void Execute(string fullSceneDataPath)
        {
            SceneInfo sceneInfo = _jsonService.CreateDataFromFile<SceneInfo>(fullSceneDataPath);
            Type type = (Assembly.GetAssembly(typeof(Scene)).GetType(typeof(Scene) + sceneInfo.Code)
               ?? throw new InvalidOperationException("Scene not found" + sceneInfo.Code));
            Scene currentScene = _diContainer.Instantiate(type, new object[] { sceneInfo }) as Scene;
            _chaptersProvider.SetCurrentScene(currentScene);
        }
    }
}
