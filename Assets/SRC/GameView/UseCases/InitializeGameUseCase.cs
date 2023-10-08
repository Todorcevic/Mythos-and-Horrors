using System.Collections.Generic;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InitializeGameUseCase
    {
        [Inject] private readonly ZoneRepository _zoneRepository;
        [Inject] private readonly AdventurerRepository _adventurerRepository;
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly CardFactory _cardFactory;
        [Inject] private readonly CardRepository _cardRepository;
        [Inject] private readonly CardGeneratorComponent _cardGeneratorComponent;
        [Inject] private readonly CardsViewManager _cardsViewManager;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        List<string> cardsToCreate = new() { "01501", "01603" };

        /*******************************************************************/
        public async Task Execute()
        {
            LoadZones();
            //LoadAdventurers();
            //LoadScene();
            LoadCards();
            LoadCardsView();
            await StartGame();
        }

        private void LoadZones()
        {
            _zoneRepository.LoadZones();
        }

        private void LoadAdventurers()
        {
            List<Adventurer> allAdventurers = _jsonService.CreateDataFromFile<List<Adventurer>>(FilesPath.JSON_ADVENTURERS_PATH);
            _adventurerRepository.LoadAdventurers(allAdventurers);
        }

        private void LoadScene()
        {

        }

        private void LoadCards()
        {
            List<CardInfo> allCardInfo = _jsonService.CreateDataFromFile<List<CardInfo>>(FilesPath.JSON_CARD_PATH);
            List<Card> allCards = _cardFactory.CreateCards(allCardInfo, cardsToCreate);
            _cardRepository.LoadCards(allCards);
        }

        private void LoadCardsView()
        {
            IReadOnlyList<Card> allCards = _cardRepository.GetAllCards();
            List<CardView> allCardView = _cardGeneratorComponent.BuildCards(allCards);
            _cardsViewManager.LoadCardsView(allCardView);
        }
        private async Task StartGame() => await _gameActionFactory.Create<StartGameAction>().Run();
    }
}
