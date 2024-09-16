using MythosAndHorrors.GameRules;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class PresenterFake : IPresenterAnimation
    {
        public Task PlayAfterAnimationWith(GameAction gameAction) => Task.CompletedTask;
        public Task PlayBeforeAnimationWith(GameAction gameAction) => Task.CompletedTask;
        public Task PlayUndoAnimationWith(GameAction gameAction) => Task.CompletedTask;

    }
}