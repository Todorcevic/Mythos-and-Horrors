using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorsProvider
    {
        [Inject] GameActionProvider _gameActionFactory;

        private readonly List<Investigator> _investigator = new();

        public List<Investigator> AllInvestigators => _investigator;
        public Investigator Leader => _investigator.First();
        public Investigator Second => _investigator[1];
        public Investigator Third => _investigator[2];
        public Investigator Fourth => _investigator[3];
        public Investigator ActiveInvestigator => _gameActionFactory.GetLastActive<PhaseGameAction>()?.ActiveInvestigator;

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

        public List<Investigator> GetInvestigatorsInThisPlace(CardPlace cardPlace)
            => _investigator.FindAll(investigator => investigator.CurrentPlace == cardPlace);

        public List<Investigator> GetInvestigatorsCanStartTurn => _investigator.FindAll(investigator => investigator.HasTurnsAvailable);
        public List<Investigator> GetInvestigatorsCanInvestigate => _investigator.FindAll(investigator => investigator.CanInvestigate);
        public List<Investigator> GetInvestigatorsCanBeHealed => _investigator.FindAll(investigator => investigator.CanBeHealed);
    }
}
