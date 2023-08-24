using System.Threading.Tasks;
using Zenject;

namespace GameRules
{
    public class StartGameAction : GameAction
    {
        [Inject] private readonly GameActionRepository _gameActionRepository;
        [Inject] private readonly CardRepository _cardRepository;
        [Inject] private readonly ZoneRepository _zoneRepository;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionRepository.Create<MoveCardGameAction>()
                   .Set(_cardRepository.GetCard("1"), _zoneRepository.GetZone(ZoneType.AssetsDeck), CardMovementType.Fast)
                   .Run();
            await _gameActionRepository.Create<MoveCardGameAction>()
                  .Set(_cardRepository.GetCard("3"), _zoneRepository.GetZone(ZoneType.LocationDiscard), CardMovementType.BasicWithPreview)
                  .Run();

            await _gameActionRepository.Create<MoveCardGameAction>()
                   .Set(_cardRepository.GetCard("5"), _zoneRepository.GetZone(ZoneType.AssetsDeck), CardMovementType.Fast)
                   .Run();
            await _gameActionRepository.Create<MoveCardGameAction>()
                   .Set(_cardRepository.GetCard("6"), _zoneRepository.GetZone(ZoneType.AssetsDeck), CardMovementType.Fast)
                   .Run();
            await _gameActionRepository.Create<MoveCardGameAction>()
                   .Set(_cardRepository.GetCard("7"), _zoneRepository.GetZone(ZoneType.AssetsDeck), CardMovementType.Fast)
                   .Run();
        }
    }
}
