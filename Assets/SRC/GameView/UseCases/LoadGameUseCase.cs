using System.Collections.Generic;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using Unity.Plastic.Newtonsoft.Json;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class LoadGameUseCase
    {
        [Inject] private readonly ZoneRepository _zoneRepository;
        [Inject] private readonly ZonesManager _zonesManager;
        [Inject] private readonly AdventurerRepository _adventurerRepository;
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly CardFactory _cardFactory;
        [Inject] private readonly CardRepository _cardRepository;
        [Inject] private readonly CardGeneratorComponent _cardGeneratorComponent;
        [Inject] private readonly CardsViewManager _cardsViewManager;

        [Inject] private readonly SaveGameUseCase _saveGameUseCase;

        List<string> cardsToCreate = new() { "01501", "01603" };

        /*******************************************************************/
        public void Execute()
        {
            LoadZones();
            LoadCardInfo();
            LoadAdventurers();
            LoadScene();
            LoadCardsView();
        }

        private void LoadZones()
        {
            List<Zone> allZones = _zonesManager.GetZones();
            _zoneRepository.SetZones(allZones);
        }

        private void LoadCardInfo()
        {
            List<CardInfo> allCardInfo = _jsonService.CreateDataFromFile<List<CardInfo>>(FilesPath.JSON_CARD_PATH);
            _cardFactory.SetCardInfo(allCardInfo);
        }

        private void LoadAdventurers()
        {
            List<Adventurer> allAdventurers = _jsonService.CreateDataFromFile<List<Adventurer>>(FilesPath.JSON_ADVENTURERS_PATH);
            _adventurerRepository.SetAdventurers(allAdventurers);
        }

        private void LoadScene()
        {
            _cardFactory.CreateCards(cardsToCreate);
        }

        private void LoadCardsView()
        {
            IReadOnlyList<Card> allCards = _cardRepository.GetAllCards();
            List<CardView> allCardView = _cardGeneratorComponent.BuildCards(allCards);
            _cardsViewManager.LoadCardsView(allCardView);
        }
    }
}
