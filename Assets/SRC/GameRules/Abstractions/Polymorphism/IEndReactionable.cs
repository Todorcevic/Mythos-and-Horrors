using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IEndReactionable
    {
        Task WhenFinish(GameAction gameAction);
    }
}
