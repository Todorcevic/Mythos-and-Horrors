using System;

namespace MythosAndHorrors.GameRules
{
    public class State
    {
        public bool InitialState { get; }
        public bool IsActive { get; private set; }
        public bool StateBeforeUpdate { get; private set; }
        public Action<bool> OnChange { get; }

        /*******************************************************************/
        public State(bool isActive, Action<bool> action = null)
        {
            InitialState = IsActive = isActive;
            OnChange = action;
        }

        /*******************************************************************/
        public void UpdateValueTo(bool isActive)
        {
            StateBeforeUpdate = IsActive;
            IsActive = isActive;
            OnChange?.Invoke(isActive);
        }
    }
}
