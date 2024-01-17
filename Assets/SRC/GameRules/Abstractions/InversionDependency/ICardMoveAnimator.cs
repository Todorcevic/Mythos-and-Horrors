using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface ICardMoveAnimator
    {
        Task MoveCardWith(MoveCardsGameAction moveCardsGameAction);
    }
}
