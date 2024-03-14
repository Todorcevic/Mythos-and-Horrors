using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IRevellable
    {
        State Revealed { get; }
        Task RevealEffect();
    }
}
