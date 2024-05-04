using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01600 : CardAdversity
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Flaw };

        public override Zone ZoneToMove => throw new System.NotImplementedException();

        protected override Task ObligationLogic()
        {
            throw new System.NotImplementedException();
        }
    }
}
