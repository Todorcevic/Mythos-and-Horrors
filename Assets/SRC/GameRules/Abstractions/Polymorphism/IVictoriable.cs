using System;
using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public interface IVictoriable
    {
        int Victory { get; }
        bool IsVictoryComplete { get; }
        IEnumerable<Investigator> InvestigatorsVictoryAffected { get; }
    }
}
