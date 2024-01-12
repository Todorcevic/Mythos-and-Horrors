using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IAnimator
    {
        Task Checking(GameAction gameAction);
    }
}
