using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Reaction<T> : IReaction where T : GameAction
    {
        public GameConditionWith<T> Condition { get; set; }
        public GameCommand<T> Command { get; set; }
        public bool IsDisable { get; private set; }
        public GameActionTime Time { get; }
        public string Description => Command.Logic.Method.Name;

        /*******************************************************************/
        public Reaction(GameConditionWith<T> condition, GameCommand<T> logic, GameActionTime time)
        {
            Condition = condition;
            Command = logic;
            Time = time;
        }
        /*******************************************************************/
        public async Task React(GameAction gameAction)
        {
            if (IsDisable) return;
            if (gameAction.IsCancel) return;
            if (gameAction is not T realGameAction) return;
            if (!Condition.IsTrueWith(realGameAction)) return;
            await Command.RunWith(realGameAction);
        }

        public void Disable() => IsDisable = true;

        public void Enable() => IsDisable = false;
    }
}
