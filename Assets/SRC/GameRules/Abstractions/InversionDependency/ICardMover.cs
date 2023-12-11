using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface ICardMover
    {
        Task MoveCardsToZoneAsync(List<Card> cards, Zone zone);

        Task MoveCardToZoneAsync(Card card, Zone gameZone);
    }
}
