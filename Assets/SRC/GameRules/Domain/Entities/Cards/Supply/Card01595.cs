using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01595 : CardSupply, IFearable
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Relic };

    }
}
