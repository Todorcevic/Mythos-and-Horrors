using System.Collections.Generic;
using MythsAndHorrors.GameRules;
using Sirenix.Utilities;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class LoadGameUseCase
    {
        [Inject] private readonly ZoneProvider _zoneProvider;
        [Inject] private readonly ZoneViewsManager _zonesManager;
        [Inject] private readonly AdventurerProvider _adventurerProvider;
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly CardFactory _cardFactory;
        [Inject] private readonly GameStateService _gameStateService;
        [Inject] private readonly CardProvider _cardProvider;
        [Inject] private readonly CardViewGeneratorComponent _cardGeneratorComponent;
        [Inject] private readonly AdventurerAreaComponent _adventurerGeneratorComponent;

        /*******************************************************************/
        public void Execute()
        {
            LoadZones();
            LoadCardInfo();
            LoadAdventurers();
            LoadScene();
            BuildCardViews();
        }

        private void LoadCardInfo()
        {
            List<CardInfo> allCardInfo = _jsonService.CreateDataFromFile<List<CardInfo>>(FilesPath.JSON_CARDINFO_PATH);
            _cardFactory.SetCardInfo(allCardInfo);
        }

        private void LoadAdventurers()
        {
            foreach (string adventurerCode in _gameStateService.AdventurersSelected)
            {
                Adventurer adventurer = _jsonService.CreateDataFromFile<Adventurer>(FilesPath.JSON_ADVENTURER_PATH(adventurerCode));
                _adventurerProvider.AddAdventurer(adventurer);
                _adventurerGeneratorComponent.BuildAdventurerArea(adventurer);
            }
        }

        private void LoadScene()
        {
            string fullSceneDataPath = FilesPath.JSON_SCENE_PATH(_gameStateService.SceneSelected);
            _gameStateService.CurrentScene = _jsonService.CreateDataFromFile<Scene>(fullSceneDataPath);
        }

        private void LoadZones() => _zoneProvider.SetZones(_zonesManager.GetSceneZones());

        private void BuildCardViews() => _cardProvider.GetAllCards().ForEach(card => _cardGeneratorComponent.BuildCard(card));
    }
}
