﻿using System;

namespace MythosAndHorrors.GameRules
{
    public class GameCondition<T>
    {
        private readonly Func<T, bool> _originalCondition;

        public Func<T, bool> ConditionLogic { get; private set; }

        /*******************************************************************/
        public GameCondition(Func<T, bool> conditionLogic)
        {
            _originalCondition = ConditionLogic = conditionLogic;
        }
        /*******************************************************************/
        public void Reset()
        {
            ConditionLogic = _originalCondition;
        }

        public void UpdateWith(Func<T, bool> condition)
        {
            ConditionLogic = condition;
        }

        public bool IsTrueWith(T element) => ConditionLogic.Invoke(element);
    }
}