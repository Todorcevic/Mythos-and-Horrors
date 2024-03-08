using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IActivable
    {
        Stat ActivationTurnsCost { get; }
        bool CanActivate();
        Task Activate();
    }
}
