using System.Collections.Generic;
using MythsAndHorrors.GameRules;
using Sirenix.Utilities;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class LoadGameUseCase
    {
        [Inject] private readonly AdventurersProvider _adventurerProvider;
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly CardFactory _cardFactory;
        [Inject] private readonly GameStateService _gameStateService;
        [Inject] private readonly LoadAreaUseCase _loadAreaUseCase;
        [Inject] private readonly CardsProvider _cardProvider;
        [Inject] private readonly CardViewGeneratorComponent _cardGeneratorComponent;

        /*******************************************************************/
        public void Execute()
        {
            LoadCardInfo();
            LoadAdventurers();
            LoadScene();
            LoadPlaces();
            BuildCardViews();
        }

        private void LoadCardInfo()
        {
            List<CardInfo> allCardInfo = _jsonService.CreateDataFromFile<List<CardInfo>>(FilesPath.JSON_CARDINFO_PATH);
            _cardFactory.SetCardInfo(allCardInfo);
        }

        private void LoadAdventurers()
        {
            for (int i = 0; i < _gameStateService.AdventurersSelected.Count; i++)
            {
                Adventurer adventurer = _jsonService.CreateDataFromFile<Adventurer>(FilesPath.JSON_ADVENTURER_PATH(_gameStateService.AdventurersSelected[i]));
                _adventurerProvider.AddAdventurer(adventurer);
            }
        }

        private void LoadScene()
        {
            string fullSceneDataPath = FilesPath.JSON_SCENE_PATH(_gameStateService.SceneSelected);
            _gameStateService.CurrentScene = _jsonService.CreateDataFromFile<Scene>(fullSceneDataPath);
        }

        private void LoadPlaces()
        {
            _loadAreaUseCase.BuildAdventurerAreas();
            _loadAreaUseCase.BuildSceneArea();
            _loadAreaUseCase.BuildPlacesArea();
        }

        private void BuildCardViews() => _cardProvider.GetAllCards().ForEach(card => _cardGeneratorComponent.BuildCard(card));
    }
}
