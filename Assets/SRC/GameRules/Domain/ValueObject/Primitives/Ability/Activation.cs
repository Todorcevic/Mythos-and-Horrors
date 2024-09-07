using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Activation<T> : ITriggered
    {
        public Card Card { get; }
        public Card CardAffected { get; }
        public Stat ActivateActionsCost { get; }
        public GameCommand<T> Logic { get; }
        public GameConditionWith<T> Condition { get; }
        public PlayActionType PlayAction { get; }
        public bool IsDisable { get; private set; }
        public bool IsFreeActivation => ActivateActionsCost.Value < 1;
        public Localization Localization { get; private set; }

        /*******************************************************************/
        public Activation(Card card, Stat activateActionsCost, GameCommand<T> logic, GameConditionWith<T> condition, PlayActionType playActionType, Card cardAffected, Localization localization)
        {
            Card = card;
            CardAffected = cardAffected;
            ActivateActionsCost = activateActionsCost;
            Logic = logic;
            Condition = condition;
            PlayAction = PlayActionType.Activate | playActionType;
            Localization = localization;
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
