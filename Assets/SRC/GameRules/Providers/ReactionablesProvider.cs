using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ReactionablesProvider
    {
        [Inject] private readonly DiContainer _diContainer;

        private readonly List<IStartReactionable> _startReactionables = new();
        private readonly List<IEndReactionable> _endReactionables = new();

        /*******************************************************************/
        public object Create(Type type, object[] args)
        {
            var newReactionable = _diContainer.Instantiate(type, args ?? new object[0]);
            if (newReactionable is IEndReactionable endReactionable) AddEndReactionable(endReactionable);
            if (newReactionable is IStartReactionable startReactionable) AddStartReactionable(startReactionable);
            return newReactionable;
        }

        /*******************************************************************/
        public async Task WhenBegin(GameAction gameAction)
        {
            foreach (IStartReactionable reaction in _startReactionables)
            {
                await reaction.WhenBegin(gameAction);
            }
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            foreach (IEndReactionable reaction in _endReactionables)
            {
                await reaction.WhenFinish(gameAction);
            }
        }

        private void AddStartReactionable(IStartReactionable reactionable)
        {
            if (_startReactionables.Contains(reactionable)) throw new InvalidOperationException("Reactionable already added");
            _startReactionables.Add(reactionable);
        }

        private void AddEndReactionable(IEndReactionable reactionable)
        {
            if (_endReactionables.Contains(reactionable)) throw new InvalidOperationException("Reactionable already added");
            _endReactionables.Add(reactionable);
        }
    }
}
