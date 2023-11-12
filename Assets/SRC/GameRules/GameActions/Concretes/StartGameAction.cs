using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class StartGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly CardProvider _cardProvider;
        [Inject] private readonly ZoneProvider _zoneProvider;

        /*******************************************************************/
        public async Task Run() => await Start();

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            MoveCardDTO moveCardDTO = new(
               _cardProvider.GetCard("01501"),
               _zoneProvider.GetZone("AdventurerDeckZone"),
               CardMovementAnimation.BasicWithPreview);
            await _gameActionRepository.Create<MoveCardGameAction>().Run(moveCardDTO);

            moveCardDTO = new(
              _cardProvider.GetCard("01560"),
              _zoneProvider.GetZone("SceneDiscardZone"),
              CardMovementAnimation.BasicWithPreview);
            await _gameActionRepository.Create<MoveCardGameAction>().Run(moveCardDTO);

            //moveCardDTO = new(
            //  _cardRepository.GetCard("5"),
            //  _zoneRepository.GetZone(ZoneType.AssetsDeck),
            //  CardMovementType.Fast);
            //await _gameActionRepository.Create<MoveCardGameAction>().Run(moveCardDTO);

            //moveCardDTO = new(
            //  _cardRepository.GetCard("6"),
            //  _zoneRepository.GetZone(ZoneType.AssetsDeck),
            //  CardMovementType.Fast);
            //await _gameActionRepository.Create<MoveCardGameAction>().Run(moveCardDTO);

            //moveCardDTO = new(
            //  _cardRepository.GetCard("7"),
            //  _zoneRepository.GetZone(ZoneType.AssetsDeck),
            //  CardMovementType.Fast);
            //await _gameActionRepository.Create<MoveCardGameAction>().Run(moveCardDTO);
        }
    }
}
