using MythsAndHorrors.GameRules;
using Sirenix.Utilities;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class PrepareGameUseCase
    {
        [Inject] private readonly SceneLoaderUseCase _sceneLoaderUseCase;
        [Inject] private readonly AdventurerLoaderUseCase _adventurerLoaderUseCase;
        [Inject] private readonly SaveDataLoaderUseCase saveDataLoaderUseCase;
        [Inject] private readonly ZoneLoaderUseCase _zoneLoaderUseCase;
        [Inject] private readonly CardsProvider _cardProvider;
        [Inject] private readonly CardViewGeneratorComponent _cardGeneratorComponent;
        [Inject] private readonly FilesPath _filesPath;
        private SaveData _saveData;

        /*******************************************************************/
        public void Execute()
        {
            LoadSaveData();
            LoadAdventurers();
            LoadScene();
            LoadZones();
            BuildCardViews();
        }

        private void LoadSaveData() => _saveData = saveDataLoaderUseCase.Execute();

        private void LoadAdventurers() =>
            _saveData.AdventurersSelected.ForEach(adventurerCode =>
            _adventurerLoaderUseCase.Execute(_filesPath.JSON_ADVENTURER_PATH(adventurerCode)));

        private void LoadScene() => _sceneLoaderUseCase.Execute(_filesPath.JSON_SCENE_PATH(_saveData.SceneSelected));

        private void LoadZones() => _zoneLoaderUseCase.Execute();

        private void BuildCardViews() => _cardProvider.AllCards.ForEach(card => _cardGeneratorComponent.BuildCard(card));
    }
}
