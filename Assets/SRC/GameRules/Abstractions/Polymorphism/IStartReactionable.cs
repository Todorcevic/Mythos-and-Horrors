using System.Threading.Tasks;

namespace GameRules
{
    public interface IStartReactionable
    {
        Task WhenBegin(GameAction gameAction);
    }
}
