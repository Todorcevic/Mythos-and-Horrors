using System;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class InvestigatorsProvider
    {
        private readonly List<Investigator> _investigator = new();

        public List<Investigator> AllInvestigators => _investigator;
        public Investigator Leader => _investigator.First();
        public Investigator Second => _investigator[1];
        public Investigator Third => _investigator[2];
        public Investigator Fourth => _investigator[3];
        public Investigator NullInvestigator => _investigator.Last();

        /*******************************************************************/
        public void AddInvestigator(Investigator investigator)
        {
            if (_investigator.Contains(investigator)) throw new InvalidOperationException("Investigator already added");
            _investigator.Add(investigator);
        }

        public int GetInvestigatorPosition(Investigator investigator) => _investigator.IndexOf(investigator) + 1;

        public Investigator GetInvestigatorWithThisZone(Zone zone)
            => _investigator.FirstOrDefault(investigator => investigator.HasThisZone(zone)) ?? NullInvestigator;

        public Investigator GetInvestigatorWithThisCard(Card card)
            => _investigator.FirstOrDefault(investigator => investigator.HasThisCard(card)) ?? NullInvestigator;

        public Investigator GetInvestigatorWithThisStat(Stat stat)
            => _investigator.FirstOrDefault(investigator => investigator.InvestigatorCard.HasThisStat(stat)) ?? NullInvestigator;
    }
}
