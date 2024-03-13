using Sirenix.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class UpdateStatGameAction : StatGameAction
    {
        /*******************************************************************/
        public UpdateStatGameAction(Stat stat, int value) : base(stat, value) { }

        public UpdateStatGameAction(Dictionary<Stat, int> statsWithValues) : base(statsWithValues) { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            StatsWithValue.ForEach(stat => stat.Key.UpdateValue(stat.Value));
            await base.ExecuteThisLogic();
        }
    }
}
