using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{

    public interface ICardMover
    {
        Task MoveCardsToZone(List<Card> cards, Zone zone);

        Task MoveCardToZone(Card card, Zone gameZone);
    }
}
