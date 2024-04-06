using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IPresenter<T>
    {
        Task PlayAnimationWith(T gameAction);
    }
}
