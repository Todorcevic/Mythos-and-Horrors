using System.Threading.Tasks;
using Tuesday.GameRules;
using Zenject;

namespace Tuesday.GameView
{
    public class InitializeGameUseCase
    {
        [Inject] private readonly ZoneFactory _zoneFactory;
        [Inject] private readonly IZoneLoader _zoneRepository;

        [Inject] private readonly DeserializeCardsInfoUseCase _deserializeCardsUseCase;
        [Inject] private readonly CardFactory _cardFactory;
        [Inject] private readonly ICardLoader _cardLoader;

        [Inject] private readonly CardRepository _cardRepository; 
        [Inject] private readonly CardGeneratorComponent _cardGeneratorComponent;
        [Inject] private readonly CardsViewManager _cardsViewManager;

        [Inject] private readonly GameActionFactory _gameActionFactory;

        /*******************************************************************/
        public async Task Execute()
        {
            _zoneRepository.LoadZones(_zoneFactory.CreateZones());

            _cardLoader.LoadCards(_cardFactory.CreateCards(_deserializeCardsUseCase.CreateFrom(FilesPath.JSON_DATA_PATH)));
          
            _cardsViewManager.LoadCardsView(_cardGeneratorComponent.BuildCards(_cardRepository.GetAllCards()));

            await _gameActionFactory.Create<StartGameAction>().Run();
        }
    }
}
