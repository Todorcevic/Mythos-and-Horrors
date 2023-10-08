using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface ICardMover
    {
        Task MoveCardsInFront(params Card[] cardIds);
        void FastMoveCardToZone(Card card, Zone gameZone);
        Task MoveCardToZone(Card card, Zone gameZone);
        Task MoveCardToZoneWithPreview(Card card, Zone gameZone);
        Task FastMoveCardToZoneWithPreview(Card card, Zone gameZone);
    }
}
