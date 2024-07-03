using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class ResetStatGameAction : UpdateStatGameAction
    {
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new UpdateStatGameAction SetWith(Stat stat, int value) => throw new NotImplementedException();

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new UpdateStatGameAction SetWith(Dictionary<Stat, int> statsWithValues) => throw new NotImplementedException();

        public ResetStatGameAction SetWith(Stat stat)
        {
            base.SetWith(stat, stat.InitialValue);
            return this;
        }

        public ResetStatGameAction SetWith(IEnumerable<Stat> stats)
        {
            base.SetWith(stats.ToDictionary(stat => stat, stat => stat.InitialValue));
            return this;
        }
    }
}
