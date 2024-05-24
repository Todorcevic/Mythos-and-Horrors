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

        private IEnumerable<IReaction> Reactions => _startReactions.Concat(_endReactions);

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
        public Reaction<T> FindReactionByLogic<T>(Func<T, Task> logic) where T : GameAction =>
            Reactions.FirstOrDefault(reaction => reaction is Reaction<T> reactionT && reactionT.Logic == logic) as Reaction<T>;

        public IReaction CreateReaction<T>(Func<T, bool> condition, Func<T, Task> logic, bool isAtStart) where T : GameAction
        {
            Reaction<T> newReaction = new(condition, logic);
            if (isAtStart) _startReactions.Add(newReaction);
            else _endReactions.Add(newReaction);
            return newReaction;
        }

        public void RemoveReaction<T>(Func<T, Task> logic) where T : GameAction
        {
            IReaction reaction = FindReactionByLogic(logic);
            _startReactions.Remove(reaction);
            _endReactions.Remove(reaction);
        }
    }
}
