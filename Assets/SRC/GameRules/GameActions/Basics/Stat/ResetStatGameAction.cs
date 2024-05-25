using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class ResetStatGameAction : UpdateStatGameAction
    {
        public ResetStatGameAction(Stat stat) : base(stat, stat.InitialValue) { }
        public ResetStatGameAction(IEnumerable<Stat> stats) : base(stats.ToDictionary(stat => stat, stat => stat.InitialValue)) { }
    }
}
