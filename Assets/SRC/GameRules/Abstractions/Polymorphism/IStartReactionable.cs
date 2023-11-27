using System.Threading.Tasks;

namespace MythsAndHorrors.EditMode
{
    public interface IStartReactionable
    {
        Task WhenBegin(GameAction gameAction);
    }
}
