using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01520 : CardSupply
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Weapon, Tag.Melee };

    }
}
