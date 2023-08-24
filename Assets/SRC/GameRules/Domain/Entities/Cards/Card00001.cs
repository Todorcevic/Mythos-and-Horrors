using System.Threading.Tasks;
using Zenject;

namespace GameRules
{
    public class Card00001 : Card, IStartReactionable, IEndReactionable
    {
        [Inject] private readonly GameActionRepository _gameActionRepository;
        [Inject] private readonly CardRepository _cardRepository;
        [Inject] private readonly ZoneRepository _zoneRepository;

        /*******************************************************************/
        public async Task WhenBegin(GameAction gameAction)
        {
            await _gameActionRepository.Create<MoveCardGameAction>()
                   .Set(_cardRepository.GetCard("3"), _zoneRepository.GetZone(ZoneType.FreeRow), CardMovementType.BasicWithPreview)
                   .Run();
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            if (gameAction is MoveCardGameAction moveCardGameAction && moveCardGameAction.Card.Id == "6")
            {
                await _gameActionRepository.Create<MoveCardGameAction>()
                      .Set(_cardRepository.GetCard("8"), _zoneRepository.GetZone(ZoneType.Rewards), CardMovementType.BasicWithPreview)
                      .Run();
            }
        }
    }
}
