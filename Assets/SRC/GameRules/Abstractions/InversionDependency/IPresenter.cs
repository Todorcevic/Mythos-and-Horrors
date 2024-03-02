using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IPresenter<T> where T : GameAction
    {
        Task PlayAnimationWith(T gameAction);
    }
}
