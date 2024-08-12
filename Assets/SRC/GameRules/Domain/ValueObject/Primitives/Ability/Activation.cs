using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Activation<T> : ITriggered
    {
        public string LocalizableCode { get; }
        public string[] LocalizableArgs { get; }
        public Card Card { get; }
        public Card CardAffected { get; }
        public Stat ActivateTurnsCost { get; }
        public GameCommand<T> Logic { get; }
        public GameConditionWith<T> Condition { get; }
        public PlayActionType PlayAction { get; }
        public string Description { get; }
        public bool IsDisable { get; private set; }
        public bool IsFreeActivation => ActivateTurnsCost.Value < 1;

        /*******************************************************************/
        public Activation(Card card, Stat activateTurnsCost, GameCommand<T> logic, GameConditionWith<T> condition, PlayActionType playActionType, Card cardAffected, string localizableCode, params string[] localizableArgs)
        {
            Card = card;
            CardAffected = cardAffected;
            ActivateTurnsCost = activateTurnsCost;
            Logic = logic;
            Condition = condition;
            PlayAction = PlayActionType.Activate | playActionType;
            LocalizableCode = localizableCode;
            LocalizableArgs = localizableArgs;
        }

        /*******************************************************************/
        public bool FullCondition(T element)
        {
            if (IsDisable) return false;
            if (!Condition.IsTrueWith(element)) return false;
            return true;
        }

        public async Task PlayFor(T element)
        {
            await Logic.RunWith(element);
        }

        public void Enable() => IsDisable = false;

        public void Disable() => IsDisable = true;
    }
}
