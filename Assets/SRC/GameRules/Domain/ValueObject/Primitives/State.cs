using System;

namespace MythosAndHorrors.GameRules
{
    public class State
    {
        public bool IsActive { get; private set; }
        public Action<bool> OnChange { get; }

        /*******************************************************************/
        internal State(bool isActive, Action<bool> action = null)
        {
            IsActive = isActive;
            OnChange = action;
        }

        /*******************************************************************/
        internal void UpdateValueTo(bool isActive)
        {
            IsActive = isActive;
            OnChange?.Invoke(isActive);
        }
    }
}
