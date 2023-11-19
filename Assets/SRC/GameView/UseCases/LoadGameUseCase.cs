using System.Collections.Generic;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class LoadGameUseCase
    {
        [Inject] private readonly ZoneProvider _zoneProvider;
        [Inject] private readonly ZonesManager _zonesManager;
        [Inject] private readonly AdventurerProvider _adventurerProvider;
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly CardFactory _cardFactory;
        [Inject] private readonly GameStateService _gameStateService;
        [Inject] private readonly CardProvider _cardProvider;
        [Inject] private readonly CardGeneratorComponent _cardGeneratorComponent;
        [Inject] private readonly CardsViewsManager _cardsViewManager;

        /*******************************************************************/
        public void Execute()
        {
            LoadZones();
            LoadCardInfo();
            LoadAdventurers();
            LoadScene();
            BuildCardViews();
        }

        private void LoadZones()
        {
            List<Zone> allZones = _zonesManager.GetZones();
            _zoneProvider.SetZones(allZones);
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
            }
        }

        private void LoadScene()
        {
            string fullSceneDataPath = FilesPath.JSON_SCENE_PATH(_gameStateService.SceneSelected);
            _gameStateService.CurrentScene = _jsonService.CreateDataFromFile<Scene>(fullSceneDataPath);
        }

        private void BuildCardViews()
        {
            IReadOnlyList<Card> allCards = _cardProvider.GetAllCards();
            List<CardView> allCardViews = _cardGeneratorComponent.BuildCards(allCards);
            _cardsViewManager.SetCardsView(allCardViews);
        }
    }
}
