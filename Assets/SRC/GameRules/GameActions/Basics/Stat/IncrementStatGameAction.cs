using System.Threading.Tasks;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

namespace MythosAndHorrors.GameRules
{
    public class IncrementStatGameAction : StatGameAction
    {
        /*******************************************************************/
        public IncrementStatGameAction(Stat stat, int value) : base(stat, value) { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Value == 0) return;
            Stat.Increase(Value);
            await base.ExecuteThisLogic();
        }
    }
}
