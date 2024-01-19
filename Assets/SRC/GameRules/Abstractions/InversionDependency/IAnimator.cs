using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IAnimator
    {
        Task PlayAnimationWith(GameAction gameAction);
    }
}
