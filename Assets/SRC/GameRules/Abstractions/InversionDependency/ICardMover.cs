using System.Threading.Tasks;

namespace MythsAndHorrors.EditMode
{
    public interface ICardMover
    {
        void MoveCardToZone(Card card, Zone gameZone);
        Task MoveCardToZoneAsync(Card card, Zone gameZone);
    }
}
