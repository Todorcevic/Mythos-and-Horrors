using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01532 : CardSupply, IDamageable, IFearable
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Miskatonic };
    }
}
