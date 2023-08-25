using System.Threading.Tasks;
using Zenject;

namespace GameRules
{
    public class MoveCardGameAction : GameAction
    {
        [Inject] private readonly ICardMover _cardMovePresenter;
        private CardMovementType _cardMovementType;

        public Card Card { get; private set; }
        public Zone Zone { get; private set; }

        /*******************************************************************/
        public async Task Run(MoveCardDTO moveCardDTO)
        {
            Card = moveCardDTO.Card;
            Zone = moveCardDTO.Zone;
            _cardMovementType = moveCardDTO.MovementType;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card.CurrentZone?.RemoveCard(Card);
            Card.MoveToZone(Zone);
            Zone.AddCard(Card);

            switch (_cardMovementType)
            {
                case CardMovementType.Basic:
                    await _cardMovePresenter.MoveCardToZone(Card.Id, Zone.ZoneType);
                    break;
                case CardMovementType.BasicWithPreview:
                    await _cardMovePresenter.MoveCardToZoneWithPreview(Card.Id, Zone.ZoneType);
                    break;
                case CardMovementType.Fast:
                    _cardMovePresenter.FastMoveCardToZone(Card.Id, Zone.ZoneType);
                    break;
                case CardMovementType.FastWithPreview:
                    await _cardMovePresenter.FastMoveCardToZoneWithPreview(Card.Id, Zone.ZoneType);
                    break;
            }
        }
    }
}
