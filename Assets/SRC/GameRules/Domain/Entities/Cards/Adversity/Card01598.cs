using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01598 : CardAdversity
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Flaw };

        public override Zone ZoneToMove => Owner.DangerZone;

        protected override async Task ObligationLogic()
        {
            await Task.CompletedTask;
        }
    }
}
