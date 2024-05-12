using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01180 : CardCreature, IStalker, ICounterAttackable
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Monster };
    }

}
