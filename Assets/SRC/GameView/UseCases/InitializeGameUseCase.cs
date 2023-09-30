using GameRules;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace GameView
{
    public class InitializeGameUseCase
    {
        [Inject] private readonly DeserializeCardsUseCase _deserializeCardsUseCase;
        [Inject] private readonly ICardLoader _cardLoader;
        [Inject] private readonly ZoneFactory _zoneFactory;
        [Inject] private readonly CardGeneratorComponent _cardGeneratorComponent;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        /*******************************************************************/
        public async Task Execute()
        {
            List<Card> allCards = _deserializeCardsUseCase.Load(FilesPath.JSON_DATA_PATH);
            _cardLoader.LoadCards(allCards);
            _zoneFactory.CreateZones();
            _cardGeneratorComponent.BuildCards();
            await _gameActionFactory.Create<StartGameAction>().Run();
        }
    }
}
