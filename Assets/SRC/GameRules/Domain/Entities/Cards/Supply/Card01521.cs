using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01521 : CardSupply, IDamageable, IFearable
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Creature };

    }
}
