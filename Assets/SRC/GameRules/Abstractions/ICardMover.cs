using System.Threading.Tasks;

namespace GameRules
{
    public interface ICardMover
    {
        Task MoveCardsInFront(params string[] cardIds);
        void FastMoveCardToZone(string cardId, ZoneType gameZone);
        Task MoveCardToZone(string cardId, ZoneType gameZone);
        Task MoveCardToZoneWithPreview(string cardId, ZoneType gameZone);
        Task FastMoveCardToZoneWithPreview(string cardId, ZoneType gameZone);
    }
}
