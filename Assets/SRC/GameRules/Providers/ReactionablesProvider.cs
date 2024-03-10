using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ReactionablesProvider
    {
        private readonly List<Func<GameAction, Task>> _onGameActionStart = new();
        private readonly List<Func<GameAction, Task>> _onGameActionEnd = new();

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
