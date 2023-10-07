using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
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

        [Inject] private readonly GameActionFactory _gameActionFactory;

        /*******************************************************************/
        public Task Execute()
        {
            LoadZones();
            LoadCards();
            LoadCardsView();
            return StartGame();
        }

        private void LoadZones() => _zoneRepository.LoadZones(_zoneFactory.CreateZones());
        private void LoadCards()
        {
            List<CardInfo> allCardInfo = _jsonService.CreateDataFromFile<List<CardInfo>>(FilesPath.JSON_DATA_PATH);

            List<CardInfo> rcoreCards = allCardInfo.Where(cardInfo => cardInfo.PackCode == "rcore").ToList();

            var dasdasda = _cardFactory.CreateCards(rcoreCards);

            _cardLoader.LoadCards(dasdasda);
        }
        private void LoadCardsView()
        {
            var asda = _cardRepository.GetAllCards();
            _cardsViewManager.LoadCardsView(_cardGeneratorComponent.BuildCards(asda));
        }
        private Task StartGame() => _gameActionFactory.Create<StartGameAction>().Run();
    }
}
