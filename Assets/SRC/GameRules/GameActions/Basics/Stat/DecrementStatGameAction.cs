using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class DecrementStatGameAction : UpdateStatGameAction
    {
        /*******************************************************************/
        public new DecrementStatGameAction SetWith(Stat stat, int value)
        {
            base.SetWith(stat, stat.RealValue - value);
            return this;
        }

        public new DecrementStatGameAction SetWith(Dictionary<Stat, int> statsWithValues)
        {
            base.SetWith(statsWithValues.ToDictionary(statNewValues => statNewValues.Key,
                statNewValues => statNewValues.Key.RealValue - statNewValues.Value));
            return this;
        }
    }
}
