using System.Threading.Tasks;

namespace MythsAndHorrors.EditMode
{
    public interface IEndReactionable
    {
        Task WhenFinish(GameAction gameAction);
    }
}
