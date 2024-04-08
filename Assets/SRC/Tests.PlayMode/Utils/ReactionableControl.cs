using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ReactionableControl
    {
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        private readonly List<Func<GameAction, Task>> _onGameActionStart = new();
        private readonly List<Func<GameAction, Task>> _onGameActionEnd = new();

        /*******************************************************************/
        public void Init()
        {
            _reactionablesProvider.SubscribeAtStart(WhenBegin);
            _reactionablesProvider.SubscribeAtEnd(WhenFinish);
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

        public void ClearAllSubscriptions()
        {
            _onGameActionStart.Clear();
            _onGameActionEnd.Clear();
        }

        private async Task WhenBegin(GameAction gameAction)
        {
            foreach (Func<GameAction, Task> handler in _onGameActionStart)
                await handler.Invoke(gameAction);
        }

        private async Task WhenFinish(GameAction gameAction)
        {
            foreach (Func<GameAction, Task> handler in _onGameActionEnd)
                await handler.Invoke(gameAction);
        }
    }
}
