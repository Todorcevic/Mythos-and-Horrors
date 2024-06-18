using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IRealReaction
    {
        GameActionTime Time { get; }
        Task React(GameAction gameAction);
    }
}
