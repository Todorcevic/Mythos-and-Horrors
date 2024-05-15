using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01528 : CardSupply, IDamageable, IFearable
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Police };
    }
}
