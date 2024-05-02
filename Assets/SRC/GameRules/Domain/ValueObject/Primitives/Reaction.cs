using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Reaction<T> : IReaction where T : GameAction
    {
        public Func<T, bool> Condition { get; }
        public Func<T, Task> Logic { get; }
        public bool IsAtStart { get; }
        public bool IsBase { get; }

        /*******************************************************************/
        public Reaction(Func<T, bool> condition, Func<T, Task> logic, bool isAtStart, bool isbase)
        {
            Condition = condition;
            Logic = logic;
            IsAtStart = isAtStart;
            IsBase = isbase;
        }
        /*******************************************************************/
        public async Task React(GameAction gameAction)
        {
            if (gameAction is not T realGameAction) return;
            if (!Condition.Invoke(realGameAction)) return;
            await Logic.Invoke(realGameAction);
        }
    }
}
