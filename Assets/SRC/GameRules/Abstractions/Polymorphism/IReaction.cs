using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IReaction
    {
        bool IsBase { get; }
        bool IsAtStart { get; }
        Task React(GameAction gameAction);
    }
}
