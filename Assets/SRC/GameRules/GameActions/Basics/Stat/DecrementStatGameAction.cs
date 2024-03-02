using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DecrementStatGameAction : StatGameAction
    {
        /*******************************************************************/
        public DecrementStatGameAction(Stat stat, int value) : base(stat, value) { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Value == 0) return;
            Stat.Decrease(Value);
            await base.ExecuteThisLogic();
        }
    }
}
