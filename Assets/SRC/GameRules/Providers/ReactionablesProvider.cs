using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ReactionablesProvider
    {
        [Inject] private readonly DiContainer _diContainer;
        private readonly List<Func<GameAction, Task>> _onGameActionStart = new();
        private readonly List<Func<GameAction, Task>> _onGameActionEnd = new();
        private readonly List<IBuffable> _buffables = new();

        public IReadOnlyList<Func<GameAction, Task>> OnGameActionStart => _onGameActionStart;
        public IReadOnlyList<Func<GameAction, Task>> OnGameActionEnd => _onGameActionEnd;

        /*******************************************************************/
        public object Create(Type type, object[] args)
        {
            var newReactionable = _diContainer.Instantiate(type, args ?? new object[0]);
            if (newReactionable is IBuffable buffable) _buffables.Add(buffable);
            return newReactionable;
        }

        /*******************************************************************/
        public void CheckActivationBuffs()
        {
            foreach (IBuffable buffable in _buffables)
            {
                buffable.ActivateBuff();
            }
        }

        public void CheckDeactivationBuffs()
        {
            foreach (IBuffable buffable in _buffables)
            {
                buffable.DeactivateBuff();
            }
        }

        /*******************************************************************/
        public void SubscribeAtStart(Func<GameAction, Task> handler)
        {
            if (_onGameActionStart.Contains(handler)) throw new InvalidOperationException("This handler is already subscribed");
            _onGameActionStart.Add(handler);
        }

        public void SubscribeAtEnd(Func<GameAction, Task> handler)
        {
            if (_onGameActionEnd.Contains(handler)) throw new InvalidOperationException("This handler is already subscribed");
            _onGameActionEnd.Add(handler);
        }

        public async Task WheBegin(GameAction gameAction)
        {
            foreach (Func<GameAction, Task> handler in _onGameActionStart)
                await handler.Invoke(gameAction);
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            foreach (Func<GameAction, Task> handler in _onGameActionEnd)
                await handler.Invoke(gameAction);
        }
    }
}
