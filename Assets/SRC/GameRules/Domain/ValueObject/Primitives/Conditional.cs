using System;

namespace MythosAndHorrors.GameRules
{
    public class Conditional
    {
        private readonly Func<bool> _condition;

        public bool? CurrentActivate { get; private set; }
        public bool IsActive => CurrentActivate ?? _condition?.Invoke() ?? false;

        /*******************************************************************/
        public Conditional(Func<bool> condition)
        {
            _condition = condition;
        }

        /*******************************************************************/
        public void UpdateActivationTo(bool? activated)
        {
            CurrentActivate = activated;
        }
    }
}
