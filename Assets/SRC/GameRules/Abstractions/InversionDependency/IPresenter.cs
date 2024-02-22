using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IPresenter<T> where T : GameAction
    {
        Task PlayAnimationWith(T gameAction);
    }
}
