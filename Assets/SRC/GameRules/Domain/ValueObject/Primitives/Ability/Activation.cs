using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Activation : ITriggered
    {
        public Card Card { get; }
        public Stat ActivateTurnsCost { get; }
        public GameCommand<Investigator> Logic { get; }
        public GameConditionWith<Investigator> Condition { get; }
        public PlayActionType PlayAction { get; }
        public string Description { get; }
        public bool IsDisable { get; private set; }
        public bool IsFreeActivation => ActivateTurnsCost.Value < 1;

        /*******************************************************************/
        public Activation(Card card, Stat activateTurnsCost, GameCommand<Investigator> logic, GameConditionWith<Investigator> condition, PlayActionType playActionType)
        {
            Card = card;
            ActivateTurnsCost = activateTurnsCost;
            Logic = logic;
            Condition = condition;
            PlayAction = PlayActionType.Activate | playActionType;
        }

        /*******************************************************************/
        public bool FullCondition(Investigator investigator)
        {
            if (IsDisable) return false;
            if (ActivateTurnsCost.Value > investigator.CurrentTurns.Value) return false;
            if (!Condition.IsTrueWith(investigator)) return false;
            return true;
        }

        public async Task PlayFor(Investigator investigator)
        {
            await Logic.RunWith(investigator);
        }

        public void Enable() => IsDisable = false;

        public void Disable() => IsDisable = true;
    }
}
