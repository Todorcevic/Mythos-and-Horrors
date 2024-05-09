using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Activation
    {
        public Stat ActivateTurnsCost { get; }
        public Func<Investigator, Task> Logic { get; }
        public Func<Investigator, bool> Condition { get; }
        public bool IsDisable { get; private set; }

        /*******************************************************************/
        public Activation(Stat activateTurnsCost, Func<Investigator, Task> logic, Func<Investigator, bool> condition)
        {
            ActivateTurnsCost = activateTurnsCost;
            Logic = logic;
            Condition = condition;
        }

        /*******************************************************************/
        public bool FullCondition(Investigator investigator)
        {
            if (IsDisable) return false;
            if (ActivateTurnsCost.Value > investigator.CurrentTurns.Value) return false;
            if (!Condition.Invoke(investigator)) return false;
            return true;
        }

        public void Enable() => IsDisable = false;

        public void Disable() => IsDisable = true;
    }
}
