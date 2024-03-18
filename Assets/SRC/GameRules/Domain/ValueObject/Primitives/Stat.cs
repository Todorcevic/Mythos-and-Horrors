namespace MythosAndHorrors.GameRules
{
    public class Stat
    {
        public int Value { get; private set; }
        public int ValueBeforeUpdate { get; private set; }

        /*******************************************************************/
        public Stat(int value)
        {
            Value = value;
        }

        /*******************************************************************/
        public void UpdateValue(int value)
        {
            ValueBeforeUpdate = Value;
            Value = value;
            if (Value < 0) Value = 0;
        }
    }
}
