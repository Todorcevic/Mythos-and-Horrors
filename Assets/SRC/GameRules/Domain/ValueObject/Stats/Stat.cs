namespace MythsAndHorrors.GameRules
{
    public class Stat
    {
        public int Value { get; private set; }
        public int MaxValue { get; private set; }

        /*******************************************************************/
        public Stat(int value, int maxValue = int.MaxValue)
        {
            Value = value;
            MaxValue = maxValue;
        }

        /*******************************************************************/
        public void Increase(int amount)
        {
            Value += amount;
            if (Value > MaxValue) Value = MaxValue;
        }

        public void Decrease(int amount)
        {
            Value -= amount;
            if (Value < 0) Value = 0;
        }

        public void ChangeMaxValue(int amount)
        {
            MaxValue = amount;
            if (Value > MaxValue) Value = MaxValue;
        }
    }
}
