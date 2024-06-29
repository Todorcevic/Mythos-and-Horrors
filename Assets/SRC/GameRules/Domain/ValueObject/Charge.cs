using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Charge
    {
        public ChargeType ChargeType { get; }
        public Stat Amount { get; }
        public bool IsVoid => Amount.Value < 1;

        /*******************************************************************/
        public Charge(int amount, ChargeType chargeType)
        {
            Amount = new Stat(amount, false);
            ChargeType = chargeType;
        }
    }
}
