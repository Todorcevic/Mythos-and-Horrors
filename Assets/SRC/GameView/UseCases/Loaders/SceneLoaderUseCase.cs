using MythosAndHorrors.GameRules;
using System;
using System.Reflection;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class SceneLoaderUseCase
    {
        [Inject] private readonly FilesPath _filesPath;
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly DiContainer _diContainer;
        [Inject] private readonly DataSaveLoaderUseCase _saveDataLoaderUseCase;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        public void Execute()
        {
            SceneInfo sceneInfo = _jsonService.CreateDataFromFile<SceneInfo>(_filesPath.JSON_SCENE_PATH(_saveDataLoaderUseCase.DataSave.SceneSelected));
            Type type = (Assembly.GetAssembly(typeof(Scene)).GetType(typeof(Scene) + sceneInfo.Code)
               ?? throw new InvalidOperationException("Scene not found" + sceneInfo.Code));
            Scene currentScene = _diContainer.Instantiate(type, new object[] { sceneInfo }) as Scene;
            _chaptersProvider.SetCurrentScene(currentScene);
        }
    }
}
