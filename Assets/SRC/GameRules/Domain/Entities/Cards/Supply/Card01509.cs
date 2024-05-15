using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01509 : CardSupply
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Tome, Tag.Item };
    }
}
