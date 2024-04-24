using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IActivable
    {
        Stat ActivateTurnsCost { get; }
        Task Activate();
        bool ConditionToActivate(Investigator investigator);
    }
}
