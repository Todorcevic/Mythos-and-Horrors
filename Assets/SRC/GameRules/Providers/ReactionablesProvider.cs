using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ReactionablesProvider
    {
        private readonly List<IReaction> _reactions = new();
        List<IReaction> Before => _reactions.Where(realReaction => realReaction.Time == GameActionTime.Before).ToList();
        List<IReaction> After => _reactions.Where(realReaction => realReaction.Time == GameActionTime.After).ToList();

        /*******************************************************************/
        public async Task WhenBegin(GameAction gameAction)
        {
            foreach (IReaction reaction in Before)
                await reaction.React(gameAction);
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            foreach (IReaction reaction in After)
                await reaction.React(gameAction);
        }

        /*******************************************************************/
        public Reaction<T> CreateReaction<T>(Func<T, bool> condition, Func<T, Task> logic, GameActionTime isAtStart) where T : GameAction
        {
            Reaction<T> newReaction = new(new GameConditionWith<T>(condition), new GameCommand<T>(logic), isAtStart);
            _reactions.Add(newReaction);
            return newReaction;
        }

        public void RemoveReaction<T>(Reaction<T> reaction) where T : GameAction
        {
            _reactions.Remove(reaction);
        }
    }
}
