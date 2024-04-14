using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorsProvider
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        private readonly List<Investigator> _investigators = new();

        public List<Investigator> AllInvestigators => _investigators.ToList();
        public IEnumerable<Investigator> AllInvestigatorsInPlay => _investigators.FindAll(investigator => investigator.IsInPlay);
        public IEnumerable<Investigator> GetInvestigatorsCanStartTurn => AllInvestigatorsInPlay.Where(investigator => investigator.HasTurnsAvailable);
        public IEnumerable<Investigator> GetInvestigatorsCanInvestigate => AllInvestigatorsInPlay.Where(investigator => investigator.CanInvestigate);
        public IEnumerable<Investigator> GetInvestigatorsCanBeHealed => AllInvestigatorsInPlay.Where(investigator => investigator.CanBeHealed);
        public Investigator Leader => AllInvestigatorsInPlay.First();
        public Investigator First => _investigators[0];
        public Investigator Second => _investigators[1];
        public Investigator Third => _investigators[2];
        public Investigator Fourth => _investigators[3];
        public Investigator GetTopInvestigatorsStrength =>
            AllInvestigatorsInPlay.OrderByDescending(investigator => investigator.Strength.Value).First();
        public Investigator ActiveInvestigator => _gameActionsProvider.CurrentPhase.ActiveInvestigator;

        /*******************************************************************/
        public void AddInvestigator(Investigator investigator)
        {
            if (_investigators.Contains(investigator)) throw new InvalidOperationException("Investigator already added");
            _investigators.Add(investigator);
        }

        /*******************************************************************/
        public int GetInvestigatorPosition(Investigator investigator)
            => _investigators.IndexOf(investigator) + 1;

        public Investigator GetInvestigatorWithThisZone(Zone zone)
            => _investigators.FirstOrDefault(investigator => investigator.HasThisZone(zone));

        public Investigator GetInvestigatorOwnerWithThisZone(Zone zone)
          => _investigators.FirstOrDefault(investigator => investigator.HasThisOwnerZone(zone));

        public Investigator GetInvestigatorWithThisCard(Card card)
            => _investigators.FirstOrDefault(investigator => investigator.HasThisCard(card));

        public Investigator GetInvestigatorWithThisStat(Stat stat)
            => _investigators.FirstOrDefault(investigator => investigator.InvestigatorCard.HasThisStat(stat));

        public IEnumerable<Investigator> GetInvestigatorsInThisPlace(CardPlace cardPlace)
            => AllInvestigatorsInPlay.Where(investigator => investigator.CurrentPlace == cardPlace);
    }
}
