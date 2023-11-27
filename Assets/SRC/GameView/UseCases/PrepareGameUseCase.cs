using System.Collections.Generic;
using MythsAndHorrors.GameRules;
using Sirenix.Utilities;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class PrepareGameUseCase
    {
        [Inject] private readonly AdventurersProvider _adventurerProvider;
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly CardFactory _cardFactory;
        [Inject] private readonly GameStateService _gameStateService;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly CardsProvider _cardProvider;
        [Inject] private readonly CardViewGeneratorComponent _cardGeneratorComponent;
        private SaveData _saveData;

        /*******************************************************************/
        public void Execute()
        {
            LoadSaveData();
            LoadCardInfo();
            LoadAdventurers();
            LoadScene();
            InitializeZones();
            BuildCardViews();
        }

        private void LoadSaveData() => _saveData = _jsonService.CreateDataFromFile<SaveData>(FilesPath.JSON_SAVE_DATA_PATH);

        private void LoadCardInfo()
        {
            List<CardInfo> allCardInfo = _jsonService.CreateDataFromFile<List<CardInfo>>(FilesPath.JSON_CARDINFO_PATH);
            _cardFactory.SetCardInfo(allCardInfo);
        }

        private void LoadAdventurers()
        {
            for (int i = 0; i < _saveData.AdventurersSelected.Count; i++)
            {
                Adventurer adventurer = _jsonService.CreateDataFromFile<Adventurer>(FilesPath.JSON_ADVENTURER_PATH(_saveData.AdventurersSelected[i]));
                _adventurerProvider.AddAdventurer(adventurer);
            }
        }

        private void LoadScene()
        {
            string fullSceneDataPath = FilesPath.JSON_SCENE_PATH(_saveData.SceneSelected);
            _gameStateService.CurrentScene = _jsonService.CreateDataFromFile<Scene>(fullSceneDataPath);
        }

        private void InitializeZones() => _zoneViewsManager.Init();

        private void BuildCardViews() => _cardProvider.GetAllCards().ForEach(card => _cardGeneratorComponent.BuildCard(card));
    }
}
