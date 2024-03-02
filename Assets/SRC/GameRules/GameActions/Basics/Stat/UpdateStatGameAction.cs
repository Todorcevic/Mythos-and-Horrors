using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class UpdateStatGameAction : StatGameAction
    {
        /*******************************************************************/
        public UpdateStatGameAction(Stat stat, int value) : base(stat, value) { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Value == Stat.Value) return;
            Stat.UpdateValue(Value);
            await base.ExecuteThisLogic();
        }
    }
}
