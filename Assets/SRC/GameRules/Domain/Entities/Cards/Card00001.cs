using System.Threading.Tasks;
using Zenject;

namespace GameRules
{
    public class Card00001 : Card, IStartReactionable, IEndReactionable
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly CardRepository _cardRepository;
        [Inject] private readonly ZoneRepository _zoneRepository;

        /*******************************************************************/
        public async Task WhenBegin(GameAction gameAction)
        {
            MoveCardDTO moveCardDTO = new(
                _cardRepository.GetCard("3"),
                _zoneRepository.GetZone(ZoneType.FreeRow),
                CardMovementType.BasicWithPreview);
            await _gameActionRepository.Create<MoveCardGameAction>().Run(moveCardDTO);
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            if (gameAction is MoveCardGameAction moveCardGameAction && moveCardGameAction.Card.Id == "6")
            {
                MoveCardDTO moveCardDTO = new(
                    _cardRepository.GetCard("8"),
                    _zoneRepository.GetZone(ZoneType.Rewards),
                    CardMovementType.BasicWithPreview);
                await _gameActionRepository.Create<MoveCardGameAction>().Run(moveCardDTO);
            }
        }
    }
}
