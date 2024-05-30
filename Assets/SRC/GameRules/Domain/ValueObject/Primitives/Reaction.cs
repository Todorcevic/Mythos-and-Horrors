using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Reaction<T> : IReaction where T : GameAction
    {
        public Func<T, bool> Condition { get; set; }
        public Func<T, Task> Logic { get; set; }
        public bool IsDisable { get; private set; }
        public string Description => Logic.Method.Name;

        /*******************************************************************/
        public Reaction(Func<T, bool> condition, Func<T, Task> logic)
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

        public void NewCondition(Func<GameAction, bool> condition)
        {
            Condition = condition;
        }
    }
}
