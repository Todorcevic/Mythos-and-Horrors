using System;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class GameAction
    {
        private static event Func<GameAction, Task> OnGameActionStart;
        private static event Func<GameAction, Task> OnGameActionEnd;
        private static GameAction _current;
        [Inject] private readonly IPresenter<GameAction> _continuousPresenter;

        public bool IsActive { get; private set; }
        public GameAction Parent { get; private set; }
        protected virtual bool CanBeExecuted => true;

        /*******************************************************************/
        public async Task Start()
        {
            if (!CanBeExecuted) return;
            IsActive = true;
            Parent = _current ?? this;
            _current = this;
            await WheBegin();
            await ExecuteThisLogic();
            await _continuousPresenter.PlayAnimationWith(this);
            await WhenFinish();
            _current = Parent ?? this;
            IsActive = false;
        }

        protected abstract Task ExecuteThisLogic();

        /*******************************************************************/
        private async Task WheBegin()
        {
            foreach (Func<GameAction, Task> handler in OnGameActionStart.GetInvocationList().Cast<Func<GameAction, Task>>())
                await handler.Invoke(this);
        }

        private async Task WhenFinish()
        {
            foreach (Func<GameAction, Task> handler in OnGameActionEnd.GetInvocationList().Cast<Func<GameAction, Task>>())
                await handler.Invoke(this);
        }

        /*******************************************************************/
        public static void SubscribeAtStart(Func<GameAction, Task> handler)
        {
            if (OnGameActionStart?.GetInvocationList().Contains(handler) ?? false)
                throw new InvalidOperationException("This handler is already subscribed");
            OnGameActionStart += handler;
        }

        public static void SubscribeAtEnd(Func<GameAction, Task> handler)
        {
            if (OnGameActionEnd?.GetInvocationList().Contains(handler) ?? false)
                throw new InvalidOperationException("This handler is already subscribed");
            OnGameActionEnd += handler;
        }
    }
}
