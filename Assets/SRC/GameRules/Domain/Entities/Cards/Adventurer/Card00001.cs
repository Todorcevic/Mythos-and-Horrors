using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card00001 : CardAdventurer, IStartReactionable, IEndReactionable
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly CardRepository _cardRepository;
        [Inject] private readonly ZoneRepository _zoneRepository;

        /*******************************************************************/
        public async Task WhenBegin(GameAction gameAction)
        {
            if (gameAction is MoveCardGameAction moveCardGameAction && moveCardGameAction.Card.Info.Code == "6")
            {
                MoveCardDTO moveCardDTO = new(
                _cardRepository.GetCard("3"),
                _zoneRepository.GetZone(ZoneType.FreeRow),
                CardMovementAnimation.BasicWithPreview);
                await _gameActionRepository.Create<MoveCardGameAction>().Run(moveCardDTO);
            }
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            if (gameAction is MoveCardGameAction moveCardGameAction && moveCardGameAction.Card.Info.Code == "00002")
            {
                MoveCardDTO moveCardDTO = new(
                    this,
                    _zoneRepository.GetZone(ZoneType.Rewards),
                    CardMovementAnimation.BasicWithPreview);
                await _gameActionRepository.Create<MoveCardGameAction>().Run(moveCardDTO);
            }
        }
    }
}
