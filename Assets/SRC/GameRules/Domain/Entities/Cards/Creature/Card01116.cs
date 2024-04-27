using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01116 : CardCreature, ITarget, IStalker
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        Investigator ITarget.Investigator => _investigatorsProvider.GetTopInvestigatorsStrength;
        public override IEnumerable<Tag> Tags => new[] { Tag.Ghoul };
    }
}
