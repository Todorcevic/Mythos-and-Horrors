using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorsProvider
    {
        [Inject] private readonly OwnersProvider _ownersProvider;
        public IEnumerable<Investigator> AllInvestigators => _ownersProvider.AllOwners.OfType<Investigator>();
        public IEnumerable<Investigator> AllInvestigatorsInPlay => AllInvestigators.Where(investigator => investigator.IsInPlay);
        public IEnumerable<Investigator> GetInvestigatorsCanStartTurn => AllInvestigatorsInPlay.Where(investigator => investigator.HasTurnsAvailable);
        public IEnumerable<Investigator> GetInvestigatorsCanInvestigate => AllInvestigatorsInPlay.Where(investigator => investigator.CanInvestigate.IsTrue);
        public IEnumerable<Investigator> GetInvestigatorsCanBeHealed => AllInvestigatorsInPlay.Where(investigator => investigator.CanBeHealed.IsTrue);
        public Investigator Leader => AllInvestigatorsInPlay.First();
        public Investigator First => AllInvestigators.ElementAt(0);
        public Investigator Second => AllInvestigators.ElementAt(1);
        public Investigator Third => AllInvestigators.ElementAt(2);
        public Investigator Fourth => AllInvestigators.ElementAt(3);
        public Investigator GetTopInvestigatorsStrength =>
            AllInvestigatorsInPlay.OrderByDescending(investigator => investigator.Strength.Value).First();

        /*******************************************************************/
        public int GetInvestigatorPosition(Investigator investigator)
            => AllInvestigators.IndexOf(investigator) + 1;

        public Investigator GetInvestigatorWithThisZone(Zone zone)
            => AllInvestigators.FirstOrDefault(investigator => investigator.HasThisZone(zone));

        public Investigator GetInvestigatorOnlyZonesOwnerWithThisZone(Zone zone)
          => AllInvestigators.FirstOrDefault(investigator => investigator.HasThisOwnerZone(zone));

        public Investigator GetInvestigatorWithThisCard(Card card)
            => AllInvestigators.FirstOrDefault(investigator => investigator.HasThisCard(card));

        public Investigator GetInvestigatorWithThisStat(Stat stat)
            => AllInvestigators.FirstOrDefault(investigator => investigator.InvestigatorCard.HasThisStat(stat));

        public IEnumerable<Investigator> GetInvestigatorsInThisPlace(CardPlace cardPlace)
            => AllInvestigatorsInPlay.Where(investigator => cardPlace != null && investigator.CurrentPlace == cardPlace);
    }
}
