using MythsAndHorrors.GameRules;
using Sirenix.Utilities;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class PrepareGameUseCase
    {
        [Inject] private readonly SceneLoaderUseCase _sceneLoader;
        [Inject] private readonly AdventurerLoaderUseCase _adventurerLoader;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly CardsProvider _cardProvider;
        [Inject] private readonly CardViewGeneratorComponent _cardGeneratorComponent;
        [Inject] private readonly FilesPath _filesPath;

        private SaveData _saveData;

        /*******************************************************************/
        public void Execute(SaveData saveData)
        {
            _saveData = saveData;
            LoadAdventurers();
            LoadScene();
            InitializeZones();
            BuildCardViews();
        }

        private void LoadAdventurers() =>
            _saveData.AdventurersSelected.ForEach(adventurerCode =>
            _adventurerLoader.Execute(_filesPath.JSON_ADVENTURER_PATH(adventurerCode)));

        private void LoadScene() => _sceneLoader.Execute(_filesPath.JSON_SCENE_PATH(_saveData.SceneSelected));

        private void InitializeZones() => _zoneViewsManager.Init();

        private void BuildCardViews() => _cardProvider.AllCards.ForEach(card => _cardGeneratorComponent.BuildCard(card));
    }
}
