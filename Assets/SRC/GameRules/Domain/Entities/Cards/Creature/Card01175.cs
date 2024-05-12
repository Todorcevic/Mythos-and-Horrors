using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01175 : CardCreature, IStalker, ITarget
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public Investigator TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
            .OrderBy(investigator => investigator.Sanity.Value).First();

        public override IEnumerable<Tag> Tags => new[] { Tag.Monster };

    }
}
