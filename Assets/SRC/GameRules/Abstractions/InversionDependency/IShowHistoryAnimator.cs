using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IShowHistoryAnimator
    {
        Task ShowHistory(ShowHistoryGameAction showHistoryGameAction);
    }
}
