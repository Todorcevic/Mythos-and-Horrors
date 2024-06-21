using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Activation<T> : ITriggered
    {
        public Card Card { get; }
        public Stat ActivateTurnsCost { get; }
        public GameCommand<T> Logic { get; }
        public GameConditionWith<T> Condition { get; }
        public PlayActionType PlayAction { get; }
        public string Description { get; }
        public bool IsDisable { get; private set; }
        public bool IsFreeActivation => ActivateTurnsCost.Value < 1;

        /*******************************************************************/
        public Activation(Card card, Stat activateTurnsCost, GameCommand<T> logic, GameConditionWith<T> condition, PlayActionType playActionType)
        {
            Card = card;
            ActivateTurnsCost = activateTurnsCost;
            Logic = logic;
            Condition = condition;
            PlayAction = PlayActionType.Activate | playActionType;
        }

        /*******************************************************************/
        public bool FullCondition(T investigator)
        {
            if (IsDisable) return false;
            if (!Condition.IsTrueWith(investigator)) return false;
            return true;
        }

        public async Task PlayFor(T investigator)
        {
            await Logic.RunWith(investigator);
        }

        public void Enable() => IsDisable = false;

        public void Disable() => IsDisable = true;
    }
}
