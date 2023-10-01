using System.Collections.Generic;
using System.Threading.Tasks;
using Tuesday.GameRules;
using Zenject;

namespace Tuesday.GameView
{
    public class InitializeGameUseCase
    {
        [Inject] private readonly ZoneFactory _zoneFactory;
        [Inject] private readonly IZoneLoader _zoneRepository;

        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly CardFactory _cardFactory;
        [Inject] private readonly ICardLoader _cardLoader;

        [Inject] private readonly CardRepository _cardRepository;
        [Inject] private readonly CardGeneratorComponent _cardGeneratorComponent;
        [Inject] private readonly CardsViewManager _cardsViewManager;

        /*******************************************************************/
        public void Execute()
        {
            LoadZones();
            LoadCards();
            LoadCardsView();
        }

        private void LoadZones() => _zoneRepository.LoadZones(_zoneFactory.CreateZones());
        private void LoadCards() => _cardLoader.LoadCards(_cardFactory.CreateCards(_jsonService.CreateDataFromFile<List<CardInfo>>(FilesPath.JSON_DATA_PATH)));
        private void LoadCardsView() => _cardsViewManager.LoadCardsView(_cardGeneratorComponent.BuildCards(_cardRepository.GetAllCards()));
    }
}
