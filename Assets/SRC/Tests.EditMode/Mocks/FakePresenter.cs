using MythosAndHorrors.GameRules;
using System.Threading.Tasks;

namespace MythosAndHorrors.EditMode.Tests
{
    public class FakePresenter<T> : IPresenter<T> where T : GameAction
    {
        public Task PlayAnimationWith(T gameAction) => Task.CompletedTask;
    }
}