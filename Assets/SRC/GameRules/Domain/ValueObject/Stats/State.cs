namespace MythsAndHorrors.GameRules
{
    public class State
    {
        public bool Value { get; private set; }

        /*******************************************************************/
        public State(bool value)
        {
            Value = value;
        }

        public void UpdateValue(bool value)
        {
            Value = value;
        }
    }
}
