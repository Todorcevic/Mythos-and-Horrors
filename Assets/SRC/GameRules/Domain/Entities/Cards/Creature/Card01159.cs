using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01159 : CardCreature, IStalker
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Creature };

    }
}
