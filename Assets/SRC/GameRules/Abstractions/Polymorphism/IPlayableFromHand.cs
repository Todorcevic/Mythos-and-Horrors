using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{

    public interface IPlayableFromHand
    {
        Stat ResourceCost { get; }
        Stat TurnsCost { get; }

        Task PlayFromHand();
        bool CanPlayFromHand();
    }
}
