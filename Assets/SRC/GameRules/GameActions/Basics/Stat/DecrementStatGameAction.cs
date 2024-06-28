using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class DecrementStatGameAction : UpdateStatGameAction
    {
        /*******************************************************************/
        public DecrementStatGameAction(Stat stat, int value) : base(stat, stat.RealValue - value) { }

        public DecrementStatGameAction(Dictionary<Stat, int> statsWithValues)
            : base(statsWithValues.ToDictionary(statNewValues => statNewValues.Key,
                statNewValues => statNewValues.Key.RealValue - statNewValues.Value))
        { }

        /*******************************************************************/
    }
}
