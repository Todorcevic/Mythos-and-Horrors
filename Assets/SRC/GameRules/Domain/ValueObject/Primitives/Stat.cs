namespace MythosAndHorrors.GameRules
{
    public class Stat
    {
        private bool _canBeNegative;
        public int Value { get; private set; }
        public int ValueBeforeUpdate { get; private set; }

        /*******************************************************************/
        public Stat(int value, bool canBeNegative)
        {
            Value = value;
            _canBeNegative = canBeNegative;
        }

        /*******************************************************************/
        public void UpdateValue(int value)
        {
            ValueBeforeUpdate = Value;
            Value = value;
            if (!_canBeNegative && Value < 0) Value = 0;
        }
    }
}
