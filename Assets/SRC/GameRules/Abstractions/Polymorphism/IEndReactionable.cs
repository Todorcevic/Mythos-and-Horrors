using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IEndReactionable
    {
        Task WhenFinish(GameAction gameAction);
    }
}
