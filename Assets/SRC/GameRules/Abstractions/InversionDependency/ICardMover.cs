using System.Threading.Tasks;

namespace Tuesday.GameRules
{
    public interface ICardMover
    {
        Task MoveCardsInFront(params Card[] cardIds);
        void FastMoveCardToZone(Card card, ZoneType gameZone);
        Task MoveCardToZone(Card card, ZoneType gameZone);
        Task MoveCardToZoneWithPreview(Card card, ZoneType gameZone);
        Task FastMoveCardToZoneWithPreview(Card card, ZoneType gameZone);
    }
}
