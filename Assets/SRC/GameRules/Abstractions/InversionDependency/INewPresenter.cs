using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface INewPresenter<T> where T : GameAction
    {
        Task PlayAnimationWith(T gameAction);
    }
}
