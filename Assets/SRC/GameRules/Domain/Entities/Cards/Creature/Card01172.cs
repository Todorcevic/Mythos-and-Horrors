using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01172 : CardCreature, IStalker
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Monster, Tag.Nightgaunt };

    }

}
