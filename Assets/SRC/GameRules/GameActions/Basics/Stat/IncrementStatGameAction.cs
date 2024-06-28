
using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class IncrementStatGameAction : UpdateStatGameAction
    {
        /*******************************************************************/
        public IncrementStatGameAction(Stat stat, int value) : base(stat, stat.RealValue + value) { }

        public IncrementStatGameAction(Dictionary<Stat, int> statsWithValues)
            : base(statsWithValues.ToDictionary(statNewValues => statNewValues.Key,
                statNewValues => statNewValues.Key.RealValue + statNewValues.Value))
        { }

        /*******************************************************************/
    }
}
