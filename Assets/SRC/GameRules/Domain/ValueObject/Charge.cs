﻿namespace MythosAndHorrors.GameRules
{
    public class Charge
    {
        public ChargeType ChargeType { get; }
        public Stat Amount { get; }
        public bool IsEmpty => Amount.Value < 1;

        /*******************************************************************/
        public Charge(int amount, ChargeType chargeType)
        {
            Amount = new Stat(amount, false);
            ChargeType = chargeType;
        }

        public Charge(Stat stat, ChargeType chargeType)
        {
            Amount = stat;
            ChargeType = chargeType;
        }
    }
}
