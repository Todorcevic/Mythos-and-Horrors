using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IReaction
    {
        bool IsAtStart { get; }
        Task React(GameAction gameAction);
    }
}
