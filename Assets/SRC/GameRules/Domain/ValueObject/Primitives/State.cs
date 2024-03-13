namespace MythosAndHorrors.GameRules
{
    public class State
    {
        public bool IsActive { get; private set; }

        /*******************************************************************/
        internal State(bool isActive)
        {
            IsActive = isActive;
        }

        /*******************************************************************/
        internal void UpdateValueTo(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
