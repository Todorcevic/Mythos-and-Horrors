using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01121b : CardCreature, IStalker, ITarget, ISpawnable
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public Investigator TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
            .OrderByDescending(investigator => investigator.Hints.Value).First();

        public CardPlace SpawnPlace => TargetInvestigator.CurrentPlace;
    }

}
