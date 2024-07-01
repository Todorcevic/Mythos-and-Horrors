
using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class IncrementStatGameAction : UpdateStatGameAction
    {
        /*******************************************************************/
        public new IncrementStatGameAction SetWith(Stat stat, int value)
        {
            base.SetWith(stat, stat.RealValue + value);
            return this;
        }

        public new IncrementStatGameAction SetWith(Dictionary<Stat, int> statsWithValues)
        {
            base.SetWith(statsWithValues.ToDictionary(statNewValues => statNewValues.Key,
                statNewValues => statNewValues.Key.RealValue + statNewValues.Value));
            return this;
        }
    }
}
