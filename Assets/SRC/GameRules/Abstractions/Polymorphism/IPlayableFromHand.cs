using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IPlayableFromHand
    {
        Stat ResourceCost { get; }
        Stat PlayFromHandTurnsCost { get; }

        Task PlayFromHand();
        bool SpecificConditionToPlayFormHand() => false;
    }
}
