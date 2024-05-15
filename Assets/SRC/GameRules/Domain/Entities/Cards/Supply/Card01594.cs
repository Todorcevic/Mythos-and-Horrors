using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01594 : CardSupply, IDamageable
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Armor };

    }
}
