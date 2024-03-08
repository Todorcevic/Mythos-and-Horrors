using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01603 : CardCreature, IStartReactionable, IEndReactionable
    {
        [Inject] private readonly GameActionProvider _gameActionRepository;
        [Inject] private readonly CardsProvider _cardRepository;

        /*******************************************************************/
        public async Task WhenBegin(GameAction gameAction)
        {
            //if (gameAction is MoveCardGameAction moveCardGameAction && moveCardGameAction.Card.Info.Code == "01501")
            //{
            //    MoveCardDTO moveCardDTO = new(
            //    _cardRepository.GetCard("01501"),
            //    _zoneRepository.GetZone(ZoneType.FreeRow),
            //    CardMovementAnimation.BasicWithPreview);
            //    await _gameActionRepository.Create<MoveCardGameAction>().Run(moveCardDTO);
            //}
            await Task.CompletedTask;
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            //if (gameAction is MoveCardGameAction moveCardGameAction && moveCardGameAction.Card.Info.Code == "01603")
            //{
            //    MoveCardDTO moveCardDTO = new(
            //        _cardRepository.GetCard("01603"),
            //        _zoneRepository.GetZone(ZoneType.Rewards),
            //        CardMovementAnimation.BasicWithPreview);
            //    await _gameActionRepository.Create<MoveCardGameAction>().Run(moveCardDTO);
            //}
            await Task.CompletedTask;
        }
    }
}
