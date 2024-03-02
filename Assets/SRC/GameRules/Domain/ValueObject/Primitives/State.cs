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

        public void UpdateValue(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
