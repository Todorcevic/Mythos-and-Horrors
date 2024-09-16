using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IPresenterAnimation
    {
        Task PlayBeforeAnimationWith(GameAction gameAction);
        Task PlayAfterAnimationWith(GameAction gameAction);
        Task PlayUndoAnimationWith(GameAction gameAction);
    }
}
