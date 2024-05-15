using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01531 : CardSupply
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Tome, Tag.Item };
    }
}
