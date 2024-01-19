using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class DecrementStatGameAction : StatGameAction
    {
        public override async Task Run(Stat stat, int value)
        {
            if (value == 0) return;
            await base.Run(stat, value);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Stat.Decrease(Value);
            await base.ExecuteThisLogic();
        }
    }
}
