using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01576 : CardSupply, IDamageable
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Creature };
    }
}
