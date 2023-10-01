using GameRules;
using System.Threading.Tasks;
using Zenject;

namespace GameView
{
    public class InitializeGameUseCase
    {
        [Inject] private readonly DeserializeCardsInfoUseCase _deserializeCardsUseCase;
        [Inject] private readonly CardFactory _cardFactory;
        [Inject] private readonly ICardLoader _cardLoader;
        [Inject] private readonly ZoneFactory _zoneFactory;
        [Inject] private readonly IZoneLoader _zoneRepository;
        [Inject] private readonly CardGeneratorComponent _cardGeneratorComponent;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        /*******************************************************************/
        public async Task Execute()
        {
            _cardLoader.LoadCards(_cardFactory.CreateCards(_deserializeCardsUseCase.CreateFrom(FilesPath.JSON_DATA_PATH)));
            _zoneRepository.LoadZones(_zoneFactory.CreateZones());
            _cardGeneratorComponent.BuildCards();
            await _gameActionFactory.Create<StartGameAction>().Run();
        }
    }
}
