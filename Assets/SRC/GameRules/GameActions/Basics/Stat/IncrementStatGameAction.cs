using Sirenix.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class IncrementStatGameAction : StatGameAction
    {
        /*******************************************************************/
        public IncrementStatGameAction(Stat stat, int value) : base(stat, value) { }

        public IncrementStatGameAction(Dictionary<Stat, int> statsWithValues) : base(statsWithValues) { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            StatsWithValue.ForEach(stat => stat.Key.Increase(stat.Value));
            await base.ExecuteThisLogic();
        }
    }
}
