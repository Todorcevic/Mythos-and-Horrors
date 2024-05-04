using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public interface IVictoriable
    {
        Stat Victory { get; }
        IEnumerable<Investigator> InvestigatorsVictoryAffected { get; }
    }
}
