using System;

namespace MythosAndHorrors.GameRules
{
    public class Conditional
    {
        private bool? _activated;

        private readonly Func<bool> _condition;

        public bool IsActive => _activated ?? _condition?.Invoke() ?? false;

        /*******************************************************************/
        public Conditional(Func<bool> condition)
        {
            _condition = condition;
        }

        /*******************************************************************/
        public void UpdateActivation(bool? activated)
        {
            _activated = activated;
        }
    }
}
