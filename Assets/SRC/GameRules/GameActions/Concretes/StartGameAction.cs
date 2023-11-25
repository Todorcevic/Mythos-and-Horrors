using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class StartGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly CardsProvider _cardProvider;
        [Inject] private readonly ZonesProvider _zoneProvider;

        /*******************************************************************/
        public async Task Run() => await Start();

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {

            await _gameActionRepository.Create<MoveCardGameAction>().Run(_cardProvider.GetCard("01501"), _zoneProvider.DangerDeckZone);

            await _gameActionRepository.Create<MoveCardGameAction>().Run(_cardProvider.GetCard("01560"), _zoneProvider.DangerDiscardZone);

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
