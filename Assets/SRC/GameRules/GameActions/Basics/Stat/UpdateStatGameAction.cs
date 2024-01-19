using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class UpdateStatGameAction : StatGameAction
    {
        public override async Task Run(Stat stat, int value)
        {
            if (value == stat.Value) return;
            await base.Run(stat, value);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Stat.UpdateValue(Value);
            await base.ExecuteThisLogic();
        }
    }
}
