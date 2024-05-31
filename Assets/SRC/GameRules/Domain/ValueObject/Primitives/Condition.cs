using System;

namespace MythosAndHorrors.GameRules
{
    public class Condition<T>
    {
        public Func<T, bool> ConditionLogic { get; private set; }

        /*******************************************************************/
        public Condition(Func<T, bool> conditionLogic)
        {
            ConditionLogic = conditionLogic;
        }
        /*******************************************************************/

        public bool Result(T element) => ConditionLogic.Invoke(element);

        public void NewCondition(Func<T, bool> condition)
        {
            ConditionLogic = condition;
        }
    }
}
