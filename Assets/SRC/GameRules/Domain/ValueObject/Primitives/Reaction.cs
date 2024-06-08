using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Reaction<T> : IReaction where T : GameAction
    {
        public GameCondition<T> Condition { get; set; }
        public GameCommand<T> Command { get; set; }
        public bool IsDisable { get; private set; }
        public string Description => Command.Logic.Method.Name;

        /*******************************************************************/
        public Reaction(GameCondition<T> condition, GameCommand<T> logic)
        {
            Condition = condition;
            Command = logic;
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
