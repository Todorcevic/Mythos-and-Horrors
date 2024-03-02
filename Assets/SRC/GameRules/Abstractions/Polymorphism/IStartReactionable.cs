using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IStartReactionable
    {
        Task WhenBegin(GameAction gameAction);
    }
}
