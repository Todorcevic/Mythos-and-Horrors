using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01555 : CardSupply
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Criminal };
    }
}
