using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class ReactionablesProvider
    {
        private readonly List<IStartReactionable> _startReactionables = new();
        private readonly List<IEndReactionable> _endReactionables = new();

        /*******************************************************************/
        public void AddReactionable(IStartReactionable reactionable)
        {
            if (_startReactionables.Contains(reactionable)) throw new InvalidOperationException("Reactionable already added");
            _startReactionables.Add(reactionable);
        }

        public void AddReactionable(IEndReactionable reactionable)
        {
            if (_endReactionables.Contains(reactionable)) throw new InvalidOperationException("Reactionable already added");
            _endReactionables.Add(reactionable);
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
    }
}
