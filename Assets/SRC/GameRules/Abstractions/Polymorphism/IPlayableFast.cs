using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IPlayableFast
    {
        Task PlayFast();
        bool CanPlayFast();
    }
}
