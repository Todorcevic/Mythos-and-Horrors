using System.Threading.Tasks;
using Zenject;

namespace GameRules
{
    public class StartGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly CardRepository _cardRepository;
        [Inject] private readonly ZoneRepository _zoneRepository;

        /*******************************************************************/
        public async Task Start() => await Run();

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            MoveCardDTO moveCardDTO = new(
               _cardRepository.GetCard("1"),
               _zoneRepository.GetZone(ZoneType.AssetsDeck),
               CardMovementType.Fast);
            await _gameActionRepository.Create<MoveCardGameAction>().Start(moveCardDTO);

            moveCardDTO = new(
              _cardRepository.GetCard("3"),
              _zoneRepository.GetZone(ZoneType.LocationDiscard),
              CardMovementType.BasicWithPreview);
            await _gameActionRepository.Create<MoveCardGameAction>().Start(moveCardDTO);

            moveCardDTO = new(
              _cardRepository.GetCard("5"),
              _zoneRepository.GetZone(ZoneType.AssetsDeck),
              CardMovementType.Fast);
            await _gameActionRepository.Create<MoveCardGameAction>().Start(moveCardDTO);

            moveCardDTO = new(
              _cardRepository.GetCard("6"),
              _zoneRepository.GetZone(ZoneType.AssetsDeck),
              CardMovementType.Fast);
            await _gameActionRepository.Create<MoveCardGameAction>().Start(moveCardDTO);

            moveCardDTO = new(
              _cardRepository.GetCard("7"),
              _zoneRepository.GetZone(ZoneType.AssetsDeck),
              CardMovementType.Fast);
            await _gameActionRepository.Create<MoveCardGameAction>().Start(moveCardDTO);
        }
    }
}
