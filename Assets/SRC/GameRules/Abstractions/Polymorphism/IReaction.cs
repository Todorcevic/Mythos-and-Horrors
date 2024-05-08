using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IReaction
    {
        Card Card { get; }
        bool IsBase { get; }
        bool IsAtStart { get; }
        Task React(GameAction gameAction);
    }
}
