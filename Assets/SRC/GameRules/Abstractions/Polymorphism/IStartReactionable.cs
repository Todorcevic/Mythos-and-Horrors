using System.Threading.Tasks;

namespace Tuesday.GameRules
{
    public interface IStartReactionable
    {
        Task WhenBegin(GameAction gameAction);
    }
}
