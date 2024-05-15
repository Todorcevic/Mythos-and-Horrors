using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01559 : CardSupply, IFearable
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Charm };

    }
}
