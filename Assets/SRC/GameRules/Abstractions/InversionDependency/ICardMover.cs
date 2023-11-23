using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface ICardMover
    {
        Task InstantMoveCardToZone(Zone gameZone, params Card[] cards);
        Task MoveCardToZone(Zone gameZone, params Card[] cards);
        //Task MoveCardToZoneWithPreview(Zone gameZone, params Card[] cards);
        //Task FastMoveCardToZoneWithPreview(Zone gameZone, params Card[] cards);
    }
}
