using MythsAndHorrors.EditMode;
using Zenject;

namespace MythsAndHorrors.PlayMode
{
    public class SceneLoaderUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly GameStateService _gameStateService;

        /*******************************************************************/
        public void Execute(string fullSceneDataPath) =>
            _gameStateService.CurrentScene = _jsonService.CreateDataFromFile<Scene>(fullSceneDataPath);

    }
}
