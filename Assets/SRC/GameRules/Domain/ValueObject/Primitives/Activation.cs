using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Activation
    {
        public Stat ActivateTurnsCost { get; }
        public GameCommand<Investigator> Logic { get; }
        public GameConditionWith<Investigator> Condition { get; }
        public PlayActionType PlayActionType { get; }
        public bool IsDisable { get; private set; }
        public bool IsFreeActivation => ActivateTurnsCost.Value < 1;

        /*******************************************************************/
        public Activation(Stat activateTurnsCost, GameCommand<Investigator> logic, GameConditionWith<Investigator> condition, PlayActionType playActionType)
        {
            ActivateTurnsCost = activateTurnsCost;
            Logic = logic;
            Condition = condition;
            PlayActionType = PlayActionType.Activate | playActionType;
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
