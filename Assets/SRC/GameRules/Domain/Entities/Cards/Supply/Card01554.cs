using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01554 : CardSupply, IDamageable, IFearable
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Criminal };

    }
}
