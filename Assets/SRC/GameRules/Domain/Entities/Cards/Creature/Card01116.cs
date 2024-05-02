using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01116 : CardCreature, ITarget, IStalker, ICounterAttackable
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        Investigator ITarget.TargetInvestigator => _investigatorsProvider.GetTopInvestigatorsStrength;
        public override IEnumerable<Tag> Tags => new[] { Tag.Ghoul };
    }
}
