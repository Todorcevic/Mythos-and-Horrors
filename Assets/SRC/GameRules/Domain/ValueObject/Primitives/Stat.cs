namespace MythosAndHorrors.GameRules
{
    public class Stat
    {
        public int Value { get; private set; }

        /*******************************************************************/
        public Stat(int value)
        {
            Value = value;
        }

        /*******************************************************************/
        public void UpdateValue(int value)
        {
            Value = value;
            if (Value < 0) Value = 0;
        }

        public void Increase(int amount)
        {
            Value += amount;
        }

        public void Decrease(int amount)
        {
            Value -= amount;
            if (Value < 0) Value = 0;
        }
    }
}
