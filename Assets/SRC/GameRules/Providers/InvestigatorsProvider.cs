using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorsProvider
    {
        [Inject] GameActionsProvider _gameActionsProvider;
        public List<Investigator> Investigators { get; private set; } = new();
        public List<Investigator> AllInvestigatorsInPlay => Investigators.FindAll(investigator => investigator.IsInPlay);
        public IEnumerable<Investigator> GetInvestigatorsCanStartTurn => AllInvestigatorsInPlay.FindAll(investigator => investigator.HasTurnsAvailable);
        public IEnumerable<Investigator> GetInvestigatorsCanInvestigate => AllInvestigatorsInPlay.FindAll(investigator => investigator.CanInvestigate);
        public IEnumerable<Investigator> GetInvestigatorsCanBeHealed => AllInvestigatorsInPlay.FindAll(investigator => investigator.CanBeHealed);
        public Investigator Leader => AllInvestigatorsInPlay[0];
        public Investigator First => Investigators[0];
        public Investigator Second => Investigators[1];
        public Investigator Third => Investigators[2];
        public Investigator Fourth => Investigators[3];
        public Investigator GetTopInvestigatorsStrength =>
            AllInvestigatorsInPlay.OrderByDescending(investigator => investigator.Strength.Value).First();
        public Investigator ActiveInvestigator => _gameActionsProvider.GetLastActive<PhaseGameAction>()?.ActiveInvestigator;

        /*******************************************************************/
        public void AddInvestigator(Investigator investigator)
        {
            if (Investigators.Contains(investigator)) throw new InvalidOperationException("Investigator already added");
            Investigators.Add(investigator);
        }

        /*******************************************************************/
        public int GetInvestigatorPosition(Investigator investigator)
            => Investigators.IndexOf(investigator) + 1;

        public Investigator GetInvestigatorWithThisZone(Zone zone)
            => Investigators.FirstOrDefault(investigator => investigator.HasThisZone(zone));

        public Investigator GetInvestigatorWithThisCard(Card card)
            => Investigators.FirstOrDefault(investigator => investigator.HasThisCard(card));

        public Investigator GetInvestigatorWithThisStat(Stat stat)
            => Investigators.FirstOrDefault(investigator => investigator.InvestigatorCard.HasThisStat(stat));

        public IEnumerable<Investigator> GetInvestigatorsInThisPlace(CardPlace cardPlace)
            => AllInvestigatorsInPlay.FindAll(investigator => investigator.CurrentPlace == cardPlace);
    }
}
