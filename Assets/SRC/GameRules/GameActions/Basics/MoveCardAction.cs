using Zenject;

namespace GameRules
{
    public class MoveCardAction : GameAction
    {
        [Inject] private readonly ICardMovePresenter _cardMovePresenter;
        private Card _card;
        private Zone _zone;

        /*******************************************************************/
        public void Set(Card card, Zone zone)
        {
            _card = card;
            _zone = zone;
        }

        public void Execute()
        {
            _card.CurrentZone?.RemoveCard(_card);
            _card.MoveToZone(_zone);
            _zone.AddCard(_card);
            _cardMovePresenter.MoveCardToZoneWithPreview(_card.Id, _zone.ZoneType);
        }
    }
}
