using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Activation
    {
        public Stat ActivateTurnsCost { get; }
        public Func<Investigator, Task> Logic { get; }
        public Func<Investigator, bool> Condition { get; }
        public bool IsBase { get; }

        /*******************************************************************/
        public Activation(Stat activateTurnsCost, Func<Investigator, Task> logic, Func<Investigator, bool> condition, bool isBase = false)
        {
            ActivateTurnsCost = activateTurnsCost;
            Logic = logic;
            Condition = condition;
            IsBase = isBase;
        }

        /*******************************************************************/
        public bool FullCondition(Investigator investigator)
        {
            if (ActivateTurnsCost.Value > investigator.CurrentTurns.Value) return false;
            if (!Condition.Invoke(investigator)) return false;
            return true;
        }
    }
}
