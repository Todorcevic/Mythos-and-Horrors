using System;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class InvestigatorsProvider
    {
        private readonly List<Investigator> _investigator = new();

        public IReadOnlyList<Investigator> AllInvestigators => _investigator;
        public Investigator Leader => _investigator.First();

        /*******************************************************************/
        public void AddInvestigator(Investigator investigator)
        {
            if (_investigator.Contains(investigator)) throw new InvalidOperationException("Investigator already added");
            _investigator.Add(investigator);
        }

        public int GetInvestigatorPosition(Investigator investigator) => _investigator.IndexOf(investigator) + 1;

        public Investigator GetInvestigatorWithThisZone(Zone zone)
            => _investigator.FirstOrDefault(investigator => investigator.HasThisZone(zone));

        public Investigator GetInvestigatorWithThisCard(Card card)
            => _investigator.FirstOrDefault(investigator => investigator.HasThisCard(card));
    }
}
