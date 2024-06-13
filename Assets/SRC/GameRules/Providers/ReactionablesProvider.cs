using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ReactionablesProvider
    {
        private readonly List<IReaction> _startReactions = new();
        private readonly List<IReaction> _endReactions = new();

        /*******************************************************************/
        public async Task WhenBegin(GameAction gameAction)
        {
            foreach (IReaction reaction in _startReactions.ToList())
                await reaction.React(gameAction);
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            foreach (IReaction reaction in _endReactions.ToList())
                await reaction.React(gameAction);
        }

        /*******************************************************************/
        public Reaction<T> CreateReaction<T>(Func<T, bool> condition, Func<T, Task> logic, bool isAtStart) where T : GameAction
        {
            Reaction<T> newReaction = new(new GameConditionWith<T>(condition), new GameCommand<T>(logic));
            if (isAtStart) _startReactions.Add(newReaction);
            else _endReactions.Add(newReaction);
            return newReaction;
        }

        public void RemoveReaction<T>(Reaction<T> reaction) where T : GameAction
        {
            _startReactions.Remove(reaction);
            _endReactions.Remove(reaction);
        }
    }
}
