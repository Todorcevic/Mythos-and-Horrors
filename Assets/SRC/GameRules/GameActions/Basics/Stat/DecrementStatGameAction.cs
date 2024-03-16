using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DecrementStatGameAction : StatGameAction
    {
        /*******************************************************************/
        public DecrementStatGameAction(Stat stat, int value) : base(stat, value) { }

        public DecrementStatGameAction(Dictionary<Stat, int> statsWithValues) : base(statsWithValues) { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            StatsWithValue.ForEach(stat => stat.Key.Decrease(stat.Value));
            await base.ExecuteThisLogic();
        }
    }
}
