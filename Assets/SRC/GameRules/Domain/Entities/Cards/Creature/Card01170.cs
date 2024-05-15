using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01170 : CardCreature, ICounterAttackable
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Humanoid, Tag.Cultist };

    }

}
