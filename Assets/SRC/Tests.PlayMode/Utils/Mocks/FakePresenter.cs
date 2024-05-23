using MythosAndHorrors.GameRules;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class FakePresenter<T> : IPresenter<T> where T : GameAction
    {
        public Task PlayAnimationWith(T gameAction) => Task.CompletedTask;
    }
}