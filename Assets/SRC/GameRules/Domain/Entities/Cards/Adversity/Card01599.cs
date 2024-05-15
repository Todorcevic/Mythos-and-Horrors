using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01599 : CardAdversity
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Madness };

        public override Zone ZoneToMove => Owner.DangerZone;


    }
}
