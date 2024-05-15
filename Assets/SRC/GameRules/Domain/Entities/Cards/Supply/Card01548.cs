using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01548 : CardSupply, IDamageable, IFearable
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Criminal };

    }
}
