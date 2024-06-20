using System;

namespace MythosAndHorrors.GameRules
{
    public class GameConditionWith<T>
    {
        private readonly Func<T, bool> _originalCondition;

        public bool IsDisabled { get; private set; }
        public Func<T, bool> ConditionLogic { get; private set; }

        /*******************************************************************/
        public GameConditionWith(Func<T, bool> conditionLogic)
        {
            _originalCondition = ConditionLogic = conditionLogic;
        }
        /*******************************************************************/
        public bool IsTrueWith(T element)
        {
            if (IsDisabled) return false;
            return ConditionLogic.Invoke(element);
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

        public void AddCondition(Func<T, bool> condition)
        {
            Func<T, bool> originalCondition = ConditionLogic;
            ConditionLogic = element => originalCondition.Invoke(element) && condition(element);
        }

        public void Enable()
        {
            IsDisabled = false;
        }

        public void Disable()
        {
            IsDisabled = true;
        }
    }
}
