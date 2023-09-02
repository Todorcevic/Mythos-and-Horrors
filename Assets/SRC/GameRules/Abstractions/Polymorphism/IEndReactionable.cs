using System.Threading.Tasks;

namespace GameRules
{
    public interface IEndReactionable
    {
        Task WhenFinish(GameAction gameAction);
    }
}
