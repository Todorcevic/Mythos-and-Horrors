using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface ICardMover
    {
        void MoveCardToZone(Card card, Zone gameZone);
        Task MoveCardToZoneAsync(Card card, Zone gameZone);
    }
}
