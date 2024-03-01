using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IPlayableFromHand
    {
        Stat ResourceCost { get; }
        Stat TurnsCost { get; }
        bool CanPlayFromHand();
        Task PlayFromHand();
    }
}
