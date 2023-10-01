using System.Threading.Tasks;

namespace Tuesday.GameRules
{
    public interface IEndReactionable
    {
        Task WhenFinish(GameAction gameAction);
    }
}
