using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Reaction<T> : IReaction where T : GameAction
    {
        public GameConditionWith<T> Condition { get; }
        public GameCommand<T> Command { get; }
        public GameActionTime Time { get; }
        public bool IsDisable { get; private set; }
        public Localization Localization { get; protected set; }

        /*******************************************************************/
        public Reaction(GameConditionWith<T> condition, GameCommand<T> logic, GameActionTime time)
        {
            Condition = condition;
            Command = logic;
            Time = time;
        }
        /*******************************************************************/
        public bool Check(GameAction gameAction, GameActionTime time)
        {
            if (IsDisable) return false;
            if (Time != time) return false;
            if (gameAction.IsCancel) return false;
            if (gameAction is not T realGameAction) return false;
            if (!Condition.IsTrueWith(realGameAction)) return false;
            return true;
        }

        public async Task React(GameAction gameAction) => await Command.RunWith((T)gameAction);

        public void Disable() => IsDisable = true;

        public void Enable() => IsDisable = false;
    }
}
