using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Reaction<T> : IReaction where T : GameAction
    {
        public Func<T, bool> Condition { get; }
        public Func<T, Task> Logic { get; }
        public bool IsAtStart { get; }

        /*******************************************************************/
        public Reaction(Func<T, bool> condition, Func<T, Task> logic, bool isAtStart)
        {
            Condition = condition;
            Logic = logic;
            IsAtStart = isAtStart;
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
