using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Reaction<T> : IReaction where T : GameAction
    {
        public Func<T, bool> Condition { get; }
        public Func<T, Task> Logic { get; }
        public bool IsDisable { get; private set; }

        /*******************************************************************/
        public Reaction( Func<T, bool> condition, Func<T, Task> logic)
        {
            Condition = condition;
            Logic = logic;
        }
        /*******************************************************************/
        public async Task React(GameAction gameAction)
        {
            if (IsDisable) return;
            if (gameAction is not T realGameAction) return;
            if (!Condition.Invoke(realGameAction)) return;
            await Logic.Invoke(realGameAction);
        }

        public void Disable() => IsDisable = true;

        public void Enable() => IsDisable = false;
    }
}
