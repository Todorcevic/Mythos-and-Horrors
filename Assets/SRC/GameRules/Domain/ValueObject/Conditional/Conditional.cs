using System;

namespace MythosAndHorrors.GameRules
{
    public class Conditional
    {
        private readonly Func<bool> _condition;

        public bool? CurrentFixedState { get; private set; }
        public bool IsTrue => CurrentFixedState ?? _condition.Invoke();
        public bool IsFalse => !IsTrue;

        /*******************************************************************/
        public Conditional(Func<bool> condition)
        {
            _condition = condition;
        }

        /*******************************************************************/
        public void UpdateActivationTo(bool? activated)
        {
            CurrentFixedState = activated;
        }

        public void ResetState()
        {
            CurrentFixedState = null;
        }
    }
}
