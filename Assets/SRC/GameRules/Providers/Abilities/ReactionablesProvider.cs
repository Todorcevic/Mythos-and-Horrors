using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ReactionablesProvider
    {
        private readonly List<IReaction> _reactions = new();

        /*******************************************************************/
        public async Task WhenInitial(GameAction gameAction)
        {
            foreach (IReaction reaction in _reactions.Where(realReaction => realReaction.Check(gameAction, GameActionTime.Initial)).ToList())
                await reaction.React(gameAction);
        }

        public async Task WhenBegin(GameAction gameAction)
        {
            foreach (IReaction reaction in _reactions.Where(realReaction => realReaction.Check(gameAction, GameActionTime.Before)).ToList())
                await reaction.React(gameAction);
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            foreach (IReaction reaction in _reactions.Where(realReaction => realReaction.Check(gameAction, GameActionTime.After)).ToList())
                await reaction.React(gameAction);
        }

        /*******************************************************************/
        public Reaction<T> CreateReaction<T>(Func<T, bool> condition, Func<T, Task> logic, GameActionTime time) where T : GameAction
        {
            Reaction<T> newReaction = new(new GameConditionWith<T>(condition), new GameCommand<T>(logic), time);
            _reactions.Add(newReaction);
            return newReaction;
        }

        public void RemoveReaction<T>(Reaction<T> reaction) where T : GameAction
        {
            _reactions.Remove(reaction);
        }
    }
}
