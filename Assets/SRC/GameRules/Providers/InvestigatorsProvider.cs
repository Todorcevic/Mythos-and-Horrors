using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class InvestigatorsProvider
    {
        [Inject] GameActionFactory _gameActionFactory;

        private readonly List<Investigator> _investigator = new();

        public List<Investigator> AllInvestigators => _investigator;
        public Investigator Leader => _investigator.First();
        public Investigator Second => _investigator[1];
        public Investigator Third => _investigator[2];
        public Investigator Fourth => _investigator[3];
        public Investigator ActiveInvestigator => _gameActionFactory.GetLastActive<OneInvestigatorTurnGameAction>()?.ActiveInvestigator;

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

        public Investigator GetInvestigatorWithThisStat(Stat stat)
            => _investigator.FirstOrDefault(investigator => investigator.InvestigatorCard.HasThisStat(stat));

        public List<Investigator> GetInvestigatorsCanStart => _investigator.FindAll(investigator => investigator.CanStartHisTurn);
    }
}
