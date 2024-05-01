using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IReaction
    {
        Task React(GameAction gameAction);
    }

    public class Reaction<T> : IReaction where T : GameAction
    {
        public Func<T, bool> Condition { get; }
        public Func<T, Task> Logic { get; }

        /*******************************************************************/
        public Reaction(Func<T, bool> condition, Func<T, Task> logic)
        {
            Condition = condition;
            Logic = logic;
        }
        /*******************************************************************/
        public async Task React(GameAction gameAction)
        {
            if (gameAction is not T t) return;
            if (!Condition.Invoke(t)) return;
            await Logic.Invoke(t);
        }
    }
}
