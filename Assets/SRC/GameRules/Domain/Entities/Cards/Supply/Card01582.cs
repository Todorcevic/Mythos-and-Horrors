using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01582 : CardSupply, IDamageable, IFearable
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally };
    }
}
