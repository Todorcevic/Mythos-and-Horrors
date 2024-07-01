using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class ResetStatGameAction : UpdateStatGameAction
    {
        public ResetStatGameAction SetWith(Stat stat)
        {
            SetWith(stat, stat.InitialValue);
            return this;
        }

        public ResetStatGameAction SetWith(IEnumerable<Stat> stats)
        {
            SetWith(stats.ToDictionary(stat => stat, stat => stat.InitialValue));
            return this;
        }
    }
}
