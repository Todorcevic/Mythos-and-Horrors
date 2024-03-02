using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Reaction
    {
        private readonly Stack<Func<GameAction, bool>> _reaction;
        public Func<Task> Logic { get; init; }

        /*******************************************************************/
        public Reaction(Func<GameAction, bool> reaction, Func<Task> logic)
        {
            _reaction = new Stack<Func<GameAction, bool>>();
            _reaction.Push(reaction);
            Logic = logic;
        }

        /*******************************************************************/
        public Task Check(GameAction gameAction)
        {
            if (_reaction.Peek().Invoke(gameAction)) return Logic.Invoke();
            return Task.CompletedTask;
        }

        public void Concat(Func<GameAction, bool> reaction)
        {
            Func<GameAction, bool> previousReaction = _reaction.Peek();
            _reaction.Push((_) => previousReaction.Invoke(_) && reaction.Invoke(_));
        }

        public void Update(Func<GameAction, bool> reaction)
        {
            _reaction.Push(reaction);
        }

        public void Remove()
        {
            _reaction.Pop();
        }
    }
}
