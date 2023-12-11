using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SceneLoaderUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly GameStateService _gameStateService;
        [Inject] private readonly DiContainer _diContainer;

        /*******************************************************************/
        public void Execute(string fullSceneDataPath)
        {
            _gameStateService.CurrentScene = _jsonService.CreateDataFromFile<Scene>(fullSceneDataPath);
            _diContainer.Inject(_gameStateService.CurrentScene);
        }
    }
}
