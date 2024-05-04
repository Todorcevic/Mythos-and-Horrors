using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{

    public interface IRevealable
    {
        State Revealed { get; }
        Task RevealEffect();
    }
}
