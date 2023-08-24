using System.Threading.Tasks;
using Zenject;

namespace GameRules
{
    public class MoveCardAction : GameAction
    {
        [Inject] private readonly ICardMovePresenter _cardMovePresenter;
        private Card _card;
        private Zone _zone;
        private CardMovementType _cardMovementType;

        /*******************************************************************/
        public MoveCardAction Set(Card card, Zone zone, CardMovementType cardMovementType = CardMovementType.Basic)
        {
            _card = card;
            _zone = zone;
            _cardMovementType = cardMovementType;
            return this;
        }

        protected override async Task Execute()
        {
            _card.CurrentZone?.RemoveCard(_card);
            _card.MoveToZone(_zone);
            _zone.AddCard(_card);

            switch (_cardMovementType)
            {
                case CardMovementType.Basic:
                    await _cardMovePresenter.MoveCardToZone(_card.Id, _zone.ZoneType);
                    break;
                case CardMovementType.BasicWithPreview:
                    await _cardMovePresenter.MoveCardToZoneWithPreview(_card.Id, _zone.ZoneType);
                    break;
                case CardMovementType.Fast:
                    _cardMovePresenter.FastMoveCardToZone(_card.Id, _zone.ZoneType);
                    break;
                case CardMovementType.FastWithPreview:
                    await _cardMovePresenter.FastMoveCardToZoneWithPreview(_card.Id, _zone.ZoneType);
                    break;
            }
        }
    }
}
