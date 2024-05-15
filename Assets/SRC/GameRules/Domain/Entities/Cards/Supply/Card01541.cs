using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01541 : CardSupply
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Relic };
    }
}
