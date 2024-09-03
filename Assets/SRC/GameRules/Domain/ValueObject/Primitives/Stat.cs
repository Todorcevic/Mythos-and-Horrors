namespace MythosAndHorrors.GameRules
{
    public class Stat
    {
        private readonly bool _canBeNegative;
        private int _value;

        public int InitialValue { get; }
        public int Value => !_canBeNegative && _value < 0 ? 0 : _value;
        public int RealValue => _value;
        public int ValueBeforeUpdate { get; private set; }


        /*******************************************************************/
        public Stat(int value, bool canBeNegative)
        {
            InitialValue = _value = value;
            _canBeNegative = canBeNegative;
        }

        /*******************************************************************/
        public void UpdateValue(int value)
        {
            ValueBeforeUpdate = _value;
            _value = value;
        }
    }
}
