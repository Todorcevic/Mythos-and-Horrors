using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IStartReactionable
    {
        Task WhenBegin(GameAction gameAction);
    }
}
