using System;

namespace MythosAndHorrors.GameRules
{
    public class Conditional<T>
    {
        private readonly Func<T, bool> _condition;

        public bool? CurrentFixedState { get; private set; }

        /*******************************************************************/
        public Conditional(Func<T, bool> condition)
        {
            _condition = condition;
        }

        /*******************************************************************/
        public bool IsTrueWith(T element) => CurrentFixedState ?? _condition.Invoke(element);

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
