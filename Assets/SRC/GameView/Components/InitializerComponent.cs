using MythsAndHorrors.GameRules;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InitializerComponent : MonoBehaviour
    {
        [InjectOptional] private readonly bool _mustBeLoaded = true;
        [Inject] private readonly PrepareGameUseCase _loadGameUseCase;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly FilesPath _filesPath;

        /*******************************************************************/
        private async void Start()
        {
            if (!_mustBeLoaded) return;

            SaveData saveData = _jsonService.CreateDataFromFile<SaveData>(_filesPath.JSON_SAVE_DATA_PATH);
            _loadGameUseCase.Execute(saveData);
            await _gameActionFactory.Create<StartGameAction>().Run();
        }
    }
}
