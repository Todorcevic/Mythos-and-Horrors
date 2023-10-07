using System.Collections.Generic;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InitializeGameUseCase
    {
        [Inject] private readonly ZoneFactory _zoneFactory;
        [Inject] private readonly ZoneRepository _zoneRepository;
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly CardFactory _cardFactory;
        [Inject] private readonly CardRepository _cardRepository;
        [Inject] private readonly CardGeneratorComponent _cardGeneratorComponent;
        [Inject] private readonly CardsViewManager _cardsViewManager;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        /*******************************************************************/
        public async Task Execute()
        {
            LoadZones();
            LoadCards();
            LoadCardsView();
            await StartGame();
        }

        private void LoadZones()
        {
            List<Zone> allZones = _zoneFactory.CreateZones();
            _zoneRepository.LoadZones(allZones);
        }
        private void LoadCards()
        {
            List<CardInfo> allCardInfo = _jsonService.CreateDataFromFile<List<CardInfo>>(FilesPath.JSON_DATA_PATH);
            List<Card> allCards = _cardFactory.CreateCards(allCardInfo, new List<string>() { "01501", "01603" });
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
