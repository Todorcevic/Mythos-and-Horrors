using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface ICardMoveAnimator
    {
        Task MoveCardWithPreviewToZone(Card card, Zone zone);
        Task MoveCardsToZone(List<Card> cards, Zone zone);
    }
}
