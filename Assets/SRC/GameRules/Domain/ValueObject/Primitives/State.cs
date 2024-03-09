namespace MythosAndHorrors.GameRules
{
    public class State
    {
        public bool IsActive { get; private set; }

        /*******************************************************************/
        public State(bool isActive)
        {
            IsActive = isActive;
        }

        /*******************************************************************/
        public void UpdateValueTo(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
