using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MoveCardGameAction : GameAction
    {
        [Inject] private readonly ICardMover _cardMovePresenter;
        private CardMovementAnimation _cardMovementType;

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
                case CardMovementAnimation.Basic:
                    await _cardMovePresenter.MoveCardToZone(Card, Zone);
                    break;
                case CardMovementAnimation.BasicWithPreview:
                    await _cardMovePresenter.MoveCardToZoneWithPreview(Card, Zone);
                    break;
                case CardMovementAnimation.Fast:
                    _cardMovePresenter.FastMoveCardToZone(Card, Zone);
                    break;
                case CardMovementAnimation.FastWithPreview:
                    await _cardMovePresenter.FastMoveCardToZoneWithPreview(Card, Zone);
                    break;
            }
        }
    }
}
