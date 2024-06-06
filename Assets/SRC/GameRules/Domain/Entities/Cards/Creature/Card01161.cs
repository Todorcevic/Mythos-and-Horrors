using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01161 : CardCreature, ITarget
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public Investigator TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
            .OrderByDescending(investigator => investigator.HealthLeft).First();
        public override IEnumerable<Tag> Tags => new[] { Tag.Ghoul, Tag.Humanoid, Tag.Monster };
    }

}
