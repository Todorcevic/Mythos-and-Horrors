using System;
using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public class Condition
    {
        private readonly Stack<Func<bool>> _conditions = new();

        public bool Result => _conditions.Peek().Invoke();

        /*******************************************************************/
        public Condition(Func<bool> condition)
        {
            _conditions.Push(condition);
        }

        public void UpdateCondition(Func<bool> condition)
        {
            _conditions.Push(condition);
        }

        //public void AddCondition(Func<bool> condition)
        //{
        //    _conditions.Push(NewCondition);

        //    bool NewCondition() => condition.Invoke() && Result;
        //}

        public void RemoveCondition()
        {
            _conditions.Pop();
        }
    }
}
