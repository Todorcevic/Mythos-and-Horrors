using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01157 : CardCreature, IStalker, IVictoriable
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Stat Victory { get; private set; }
        public IEnumerable<Investigator> InvestigatorsVictoryAffected => _investigatorsProvider.AllInvestigators;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Victory = CreateStat(10);
        }
        /*******************************************************************/
    }
}
